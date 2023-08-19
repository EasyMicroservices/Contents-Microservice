using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.ContentsMicroservice.Contracts.Requests
{
    public class CreateContentRequestContract
    {
        public long LanguageId { get; set; }
        public long CategoryId { get; set; }
        public string Data { get; set; }
        public string UniqueIdentity { get; set; }
    }
}
