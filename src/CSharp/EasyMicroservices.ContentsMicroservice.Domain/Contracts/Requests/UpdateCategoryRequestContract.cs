using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.ContentsMicroservice.Contracts.Requests
{
    public class UpdateCategoryRequestContract
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string UniqueIdentity { get; set; }
    }
}
