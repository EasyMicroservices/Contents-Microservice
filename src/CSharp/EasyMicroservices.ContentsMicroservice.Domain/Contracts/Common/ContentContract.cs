using System;
using System.Collections.Generic;
using System.Linq;
using EasyMicroservices.Cores.Interfaces;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.ContentsMicroservice.Contracts.Common
{
    public class ContentContract : IUniqueIdentitySchema, IDateTimeSchema, ISoftDeleteSchema
    {
        public long Id { get; set; }
        public long LanguageId { get; set; }
        public long CategoryId { get; set; }
        public string Data { get; set; }
        public string UniqueIdentity { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? ModificationDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public LanguageContract Language { get; set; }
    }
}
