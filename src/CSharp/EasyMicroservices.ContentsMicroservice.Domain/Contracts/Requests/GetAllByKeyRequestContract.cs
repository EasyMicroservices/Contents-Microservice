using EasyMicroservices.Cores.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.ContentsMicroservice.Contracts.Requests
{
    public class GetAllByKeyRequestContract : IUniqueIdentitySchema
    {
        public string Key { get; set; }
        public string UniqueIdentity { get; set; }
    }
}
