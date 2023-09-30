using EasyMicroservices.ContentsMicroservice.Contracts.Common;
using EasyMicroservices.ContentsMicroservice.Contracts.Requests;
using EasyMicroservices.ContentsMicroservice.Database.Entities;
using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.AspEntityFrameworkCoreApi.Interfaces;
using EasyMicroservices.Cores.Contracts.Requests;
using EasyMicroservices.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyMicroservices.QuestionsMicroservice.WebApi.Controllers
{
    public class ContentController : SimpleQueryServiceController<ContentEntity, CreateContentRequestContract, UpdateContentRequestContract, ContentContract, long>
    {
        readonly IUnitOfWork unitOfWork;
        public ContentController(IUnitOfWork _unitOfWork) : base(_unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public override async Task<MessageContract<long>> Add(CreateContentRequestContract request, CancellationToken cancellationToken = default)
        {
            using var categorylogic = unitOfWork.GetLongContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract>();
            using var languageLogic = unitOfWork.GetLongContractLogic<LanguageEntity, LanguageContract>();
            var checkLanguageId = await languageLogic.GetById(new GetIdRequestContract<long>() { Id = request.LanguageId });
            var checkCategoryId = await categorylogic.GetById(new GetIdRequestContract<long>() { Id = request.CategoryId });
            if (checkLanguageId.IsSuccess && checkCategoryId.IsSuccess)
                return await base.Add(request, cancellationToken);
            return (EasyMicroservices.ServiceContracts.FailedReasonType.Incorrect, "Language or Categoryid is incorrect");
        }

        public override async Task<MessageContract<ContentContract>> Update(UpdateContentRequestContract request, CancellationToken cancellationToken = default)
        {
            using var categorylogic = unitOfWork.GetLongContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract>();
            using var languageLogic = unitOfWork.GetLongContractLogic<LanguageEntity, LanguageContract>();
            var checkLanguageId = await languageLogic.GetById(new GetIdRequestContract<long>() { Id = request.LanguageId });
            var checkCategoryId = await categorylogic.GetById(new GetIdRequestContract<long>() { Id = request.CategoryId });
            if (checkLanguageId.IsSuccess && checkCategoryId.IsSuccess)
                return await base.Update(request, cancellationToken);
            return (EasyMicroservices.ServiceContracts.FailedReasonType.Incorrect, "Language or Categoryid is incorrect");

        }

        [HttpPost]
        public async Task<MessageContract<ContentContract>> GetByLanguage(GetByLanguageRequestContract request)
        {
            using var categorylogic = unitOfWork.GetLongContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract>();
            var getCategoryResult = await categorylogic.GetBy(x => x.Key == request.Key, query => query.Include(x => x.Contents).ThenInclude(x => x.Language));
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
        public async Task<ListMessageContract<ContentContract>> GetAllByKey(GetAllByKeyRequestContract request)
        {
            using var categorylogic = unitOfWork.GetLongContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract>();
            var getCategoryResult = await categorylogic.GetBy(x => x.Key == request.Key, query => query.Include(x => x.Contents).ThenInclude(x => x.Language));
            if (!getCategoryResult)
                return getCategoryResult.ToListContract<ContentContract>();

            return getCategoryResult.Result.Contents;
        }

        [HttpPost]
        public async Task<MessageContract<CategoryContract>> AddContentWithKey(AddContentWithKeyRequestContract request)
        {
            using var categorylogic = unitOfWork.GetLongContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract>();
            using var contentlogic = unitOfWork.GetLongContractLogic<ContentEntity, CreateContentRequestContract, UpdateContentRequestContract, ContentContract>();
            using var languageLogic = unitOfWork.GetLongContractLogic<LanguageEntity, LanguageContract>();

            var getCategoryResult = await categorylogic.GetBy(x => x.Key == request.Key);
            if (getCategoryResult.IsSuccess)
                return (FailedReasonType.Duplicate, "Category already exists.");

            var languages = await languageLogic.GetAll();
            var notFoundLanguages = request.LanguageData.Select(x => x.Language).Except(languages.Result.Select(o => o.Name));

            if (!notFoundLanguages.Any())
            {
                var addCategoryResult = await categorylogic.Add(new CreateCategoryRequestContract
                {
                    Key = request.Key,
                });

                if (!addCategoryResult.IsSuccess)
                    return addCategoryResult.ToContract<CategoryContract>();

                foreach (var item in request.LanguageData)
                {
                    var languageId = languages.Result.FirstOrDefault(o => o.Name == item.Language)?.Id;
                    if (!languageId.HasValue)
                        return (FailedReasonType.NotFound, $"Language {item.Language} not found!");

                    var addContentResult = await contentlogic.Add(new CreateContentRequestContract
                    {
                        CategoryId = addCategoryResult.Result,
                        LanguageId = languageId.Value,
                        Data = item.Data
                    });
                }

                var addedCategoryResult = await categorylogic.GetById(new GetIdRequestContract<long>
                {
                    Id = addCategoryResult.Result
                });

                return addedCategoryResult.Result;
            }
            return (FailedReasonType.Incorrect, $"This languages are not registered in the content server: {string.Join(", ", notFoundLanguages)}");
        }

        [HttpPost]
        public async Task<MessageContract> UpdateContentWithKey(AddContentWithKeyRequestContract request)
        {
            using var categorylogic = unitOfWork.GetLongContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract>();
            using var contentlogic = unitOfWork.GetLongContractLogic<ContentEntity, CreateContentRequestContract, UpdateContentRequestContract, ContentContract>();
            using var languageLogic = unitOfWork.GetLongContractLogic<LanguageEntity, LanguageContract>();
            var getCategoryResult = await categorylogic.GetBy(x => x.Key == request.Key, query => query.Include(x => x.Contents).ThenInclude(x => x.Language));
            if (!getCategoryResult)
                return getCategoryResult;
            var contents = getCategoryResult.Result.Contents;
            foreach (var content in contents)
            {
                if (request.LanguageData.Any(o => o.Language == content.Language.Name))
                {
                    var response = await contentlogic.Update(new UpdateContentRequestContract
                    {
                        Id = content.Id,
                        CategoryId = content.CategoryId,
                        LanguageId = content.LanguageId,
                        UniqueIdentity = content.UniqueIdentity,

                        Data = request.LanguageData.FirstOrDefault(o => o.Language == content.Language.Name).Data
                    });

                    if (!response.IsSuccess)
                        return response.ToContract();
                }
            }

            foreach (var languageData in request.LanguageData)
            {
                if (!contents.Any(o => o.Language.Name == languageData.Language))
                {
                    var language = await languageLogic.GetBy(o => o.Name == languageData.Language);

                    if (!language)
                        continue;

                    var response = await contentlogic.Add(new CreateContentRequestContract
                    {
                        CategoryId = getCategoryResult.Result.Id,
                        LanguageId = language.Result.Id,
                        UniqueIdentity = language.Result.UniqueIdentity,

                        Data = languageData.Data
                    });

                    if (!response)
                        return response.ToContract();
                }
            }

            return true;
        }

    }
}