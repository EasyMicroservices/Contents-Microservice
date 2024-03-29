﻿using EasyMicroservices.ContentsMicroservice.Contracts.Common;
using EasyMicroservices.ContentsMicroservice.Contracts.Requests;
using EasyMicroservices.ContentsMicroservice.Database.Entities;
using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.AspEntityFrameworkCoreApi.Interfaces;
using EasyMicroservices.Cores.Database.Interfaces;
using EasyMicroservices.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace EasyMicroservices.ContentsMicroservice.WebApi.Controllers
{
    public class CategoryController : SimpleQueryServiceController<CategoryEntity, CreateCategoryRequestContract, UpdateCategoryRequestContract, CategoryContract, long>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<MessageContract<CategoryContract>> HasKey(IsKeyExistRequestContract request)
        {

            return await _unitOfWork.GetLongContractLogic<CategoryEntity, CategoryContract>().GetByUniqueIdentity(request,
                Cores.DataTypes.GetUniqueIdentityType.All,
                query => query.Where(x => x.Key == request.Key)
            );

        }
    }
}
