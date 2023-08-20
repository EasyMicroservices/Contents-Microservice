using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.ContentsMicroservice.Contracts.Requests
{
    public class CreateLanguageRequestContract
    {
        public string Name { get; set; }
        public string UniqueIdentity { get; set; }
    }
}
