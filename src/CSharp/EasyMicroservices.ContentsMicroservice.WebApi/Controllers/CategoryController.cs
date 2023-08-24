using EasyMicroservices.ContentsMicroservice.Contracts.Common;
using EasyMicroservices.ContentsMicroservice.Contracts.Requests;
using EasyMicroservices.ContentsMicroservice.Database.Entities;
using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.Database.Interfaces;

namespace EasyMicroservices.ContentsMicroservice.WebApi.Controllers
{
    public class CategoryController : SimpleQueryServiceController<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract, long>
    {
        private readonly IContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract, long> _contractlogic;

        public CategoryController(IContractLogic<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract, long> contractLogic) : base(contractLogic)
        {
            _contractlogic = contractLogic;
        }
    }
}
