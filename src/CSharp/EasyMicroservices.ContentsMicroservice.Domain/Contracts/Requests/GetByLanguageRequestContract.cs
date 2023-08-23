using EasyMicroservices.Cores.Interfaces;

namespace EasyMicroservices.ContentsMicroservice.Contracts.Requests
{
    public class GetByLanguageRequestContract
    {
        public string Language { get; set; }
        public string Key { get; set; }
    }
}
