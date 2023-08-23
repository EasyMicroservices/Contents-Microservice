﻿using EasyMicroservices.ContentsMicroservice.Contracts.Common;
using EasyMicroservices.ContentsMicroservice.Contracts.Requests;
using EasyMicroservices.ContentsMicroservice.Database.Entities;
using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.Database.Interfaces;
using EasyMicroservices.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace EasyMicroservices.ContentsMicroservice.WebApi.Controllers
{
    public class LanguageController : SimpleQueryServiceController<LanguageEntity, CreateLanguageRequestContract, UpdateLanguageRequestContract, LanguageContract, long>
    {
        private readonly IContractLogic<LanguageEntity, CreateLanguageRequestContract, UpdateLanguageRequestContract, LanguageContract, long> _contractlogic;

        public LanguageController(IContractLogic<LanguageEntity, CreateLanguageRequestContract, UpdateLanguageRequestContract, LanguageContract, long> contractLogic) : base(contractLogic)
        {
            _contractlogic = contractLogic;
        }

        [HttpPost]
        public async Task<MessageContract> HasLanguage(HasLanguageRequestContract hasLanguageRequest)
        {
            return await _contractlogic.GetBy(x => x.Name == hasLanguageRequest.Language);
        }
    }
}