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

        public ContentController(IContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract, long> categorylogic, IContractLogic<LanguageEntity, CreateLanguageRequestContract, UpdateLanguageRequestContract, LanguageContract, long> languagelogic, IContractLogic<ContentEntity, CreateContentRequestContract, UpdateContentRequestContract, ContentContract, long> contractLogic) : base(contractLogic)
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
            return (EasyMicroservices.ServiceContracts.FailedReasonType.Empty, "LaguageId or Categoryid is incorrect");
        }
        public override async Task<MessageContract<ContentContract>> Update(UpdateContentRequestContract request, CancellationToken cancellationToken = default)
        {
            var checkLanguageId = await _languagelogic.GetById(new GetIdRequestContract<long>() { Id = request.LanguageId });
            var checkCategoryId = await _categorylogic.GetById(new GetIdRequestContract<long>() { Id = request.CategoryId });
            if (checkLanguageId.IsSuccess && checkCategoryId.IsSuccess)
                return await base.Update(request, cancellationToken);
            return (EasyMicroservices.ServiceContracts.FailedReasonType.Empty, "LaguageId or Categoryid is incorrect");

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
    }
}
