using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.Database.Interfaces;
using EasyMicroservices.ContentsMicroservice.Contracts.Common;
using EasyMicroservices.ContentsMicroservice.Contracts.Requests;
using EasyMicroservices.ContentsMicroservice.Database.Entities;
using EasyMicroservices.ServiceContracts;
using EasyMicroservices.Cores.Contracts.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace EasyMicroservices.QuestionsMicroservice.WebApi.Controllers
{
    public class ContentController : SimpleQueryServiceController<ContentEntity, CreateContentRequestContract, UpdateContentRequestContract, ContentContract, long>
    {
        private readonly IContractLogic<ContentEntity, CreateContentRequestContract, UpdateContentRequestContract, ContentContract, long> _contractlogic;
        private readonly IContractLogic<LanguageEntity, CreateLanguageRequestContract, UpdateLanguageRequestContract, LanguageContract, long> _languagelogic;
        private readonly IContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract, long> _categorylogic;

        public ContentController(IContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract, long> categorylogic , IContractLogic<LanguageEntity, CreateLanguageRequestContract, UpdateLanguageRequestContract, LanguageContract, long> languagelogic , IContractLogic<ContentEntity, CreateContentRequestContract, UpdateContentRequestContract, ContentContract, long> contractLogic) : base(contractLogic)
        {
            _contractlogic = contractLogic;
            _languagelogic = languagelogic;
            _categorylogic = categorylogic;
        }
        public override async Task<MessageContract<long>> Add(CreateContentRequestContract request, CancellationToken cancellationToken = default)
        {
            var checkLanguageId = await _languagelogic.GetById(new GetIdRequestContract<long>() { Id = request.LanguageId });
            var checkCategoryId = await _categorylogic.GetById(new GetIdRequestContract<long>() { Id = request.CategoryId });
            if (checkLanguageId.IsSuccess && checkCategoryId.IsSuccess)
                return await base.Add(request, cancellationToken);
            return (EasyMicroservices.ServiceContracts.FailedReasonType.Incorrect, "Language or Categoryid is incorrect");
        }
        public override async Task<MessageContract<ContentContract>> Update(UpdateContentRequestContract request, CancellationToken cancellationToken = default)
        {
            var checkLanguageId = await _languagelogic.GetById(new GetIdRequestContract<long>() { Id = request.LanguageId });
            var checkCategoryId = await _categorylogic.GetById(new GetIdRequestContract<long>() { Id = request.CategoryId });
            if (checkLanguageId.IsSuccess && checkCategoryId.IsSuccess)
                return await base.Update(request, cancellationToken);
            return (EasyMicroservices.ServiceContracts.FailedReasonType.Incorrect, "Language or Categoryid is incorrect");

        }

        [HttpPost]
        public async Task<MessageContract<ContentContract>> GetByLanguage(GetByLanguageRequestContract request)
        {
            var getCategoryResult = await _categorylogic.GetBy(x => x.Key == request.Key, query => query.Include(x => x.Contents).ThenInclude(x => x.Language));
            if (!getCategoryResult)
                return getCategoryResult.ToContract<ContentContract>();
            var contentResult = getCategoryResult.Result.Contents.FirstOrDefault(x => x.Language.Name.Equals(request.Language, StringComparison.OrdinalIgnoreCase));
            if (contentResult == null)
                contentResult = getCategoryResult.Result.Contents.FirstOrDefault();
            if (contentResult == null)
                return (FailedReasonType.NotFound, $"Content {request.Key} by language {request.Language} not found!");

            return contentResult;
        }

        [HttpPost]
        public async Task<MessageContract<CategoryContract>> AddContentWithKey(AddContentWithKeyRequestContract request)
        {
            var getCategoryResult = await _categorylogic.GetBy(x => x.Key == request.Key);
            if (getCategoryResult.IsSuccess)
                return (FailedReasonType.Incorrect, $"{getCategoryResult.Result.Key} category is already exists.");

            var languages = await _languagelogic.GetAll();
            var notFoundLanguages = request.LanguageData.Select(x => x.Language).Except(languages.Result.Select(o => o.Name));

            if (!notFoundLanguages.Any())
            {
                var addCategoryResult = await _categorylogic.Add(new CreateCategoryRequestContract { 
                    Key = request.Key,
                });

                if (!addCategoryResult.IsSuccess)
                    return addCategoryResult.ToContract<CategoryContract>();

                foreach(var item in request.LanguageData)
                {
                    var languageId = languages.Result.FirstOrDefault(o => o.Name == item.Language)?.Id;
                    if (!languageId.HasValue)
                        return (FailedReasonType.Unknown, "An error has occured!");


                    var addContentResult = _contractlogic.Add(new CreateContentRequestContract
                    {
                        CategoryId = addCategoryResult.Result,
                        LanguageId = languageId.Value,
                        Data = item.Data
                    });
                }

                return addCategoryResult.ToContract<CategoryContract>();
            }

            return (FailedReasonType.Incorrect, $"This languages are not registered in the content server: {string.Join(", ", notFoundLanguages)}");
        }


        [HttpPost]
        public async Task<MessageContract> UpdateContentWithKey(AddContentWithKeyRequestContract request)
        {
            var getCategoryResult = await _categorylogic.GetBy(x => x.Key == request.Key, query => query.Include(x => x.Contents).ThenInclude(x => x.Language));
            if (getCategoryResult.IsSuccess)
            {
                if(getCategoryResult.Result.Contents.Any())
                {
                    var contents = getCategoryResult.Result.Contents;
                    foreach (var content in contents)
                    {
                        if (request.LanguageData.Any(o => o.Language == content.Language.Name))
                        {
                            var response = await _contractlogic.Update(new UpdateContentRequestContract
                            {
                                Id = content.Id,
                                CategoryId = content.CategoryId,
                                LanguageId = content.LanguageId,
                                UniqueIdentity = content.UniqueIdentity,

                                Data = request.LanguageData.FirstOrDefault(o => o.Language == content.Language.Name).Data
                            }); 
                            
                            if (!response.IsSuccess)
                                return (FailedReasonType.Unknown, "An error has occured");

                        }
                    }

                    return true;
                }
            }

            return (FailedReasonType.Incorrect, $"{getCategoryResult.Result.Key} category doesn't exists");
        }
    }
}
