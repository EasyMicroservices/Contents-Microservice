using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.ContentsMicroservice.Contracts.Requests
{
    public class UpdateLanguageRequestContract
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string UniqueIdentity { get; set; }
    }
}
