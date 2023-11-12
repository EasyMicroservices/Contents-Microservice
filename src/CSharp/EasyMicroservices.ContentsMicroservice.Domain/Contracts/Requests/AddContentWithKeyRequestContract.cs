using EasyMicroservices.ContentsMicroservice.Contracts.Common;
using EasyMicroservices.Cores.Interfaces;
using System.Collections.Generic;

namespace EasyMicroservices.ContentsMicroservice.Contracts.Requests
{
    public class AddContentWithKeyRequestContract : IUniqueIdentitySchema
    {
        public string Key { get; set; }
        public string UniqueIdentity { get; set; }
        public List<LanguageDataContract> LanguageData { get; set; }
    }
}
