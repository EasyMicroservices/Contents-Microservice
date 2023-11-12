using EasyMicroservices.Cores.Interfaces;

namespace EasyMicroservices.ContentsMicroservice.Contracts.Requests
{
    public class GetByLanguageRequestContract : IUniqueIdentitySchema
    {
        public string Language { get; set; }
        public string Key { get; set; }
        public string UniqueIdentity { get; set; }
    }
}
