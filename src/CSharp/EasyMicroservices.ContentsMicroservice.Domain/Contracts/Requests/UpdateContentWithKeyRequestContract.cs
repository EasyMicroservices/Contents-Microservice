using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyMicroservices.ContentsMicroservice.Contracts.Common;

namespace EasyMicroservices.ContentsMicroservice.Contracts.Requests
{
    public class UpdateContentWithKeyRequestContract
    {
        public string Key { get; set; }
        public List<LanguageDataContract> LanguageData { get; set; }
    }
}
