using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.Database.Interfaces;
using EasyMicroservices.ContentsMicroservice.Contracts.Common;
using EasyMicroservices.ContentsMicroservice.Contracts.Requests;
using EasyMicroservices.ContentsMicroservice.Database.Entities;
using EasyMicroservices.ServiceContracts;
using EasyMicroservices.Cores.Contracts.Requests;

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
    }
}
