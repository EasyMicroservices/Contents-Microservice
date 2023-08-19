using EasyMicroservices.ContentsMicroservice.Database.Schemas;
using EasyMicroservices.Cores.Interfaces;
using System.Collections.Generic;

namespace EasyMicroservices.ContentsMicroservice.Database.Entities
{
    public class LanguageEntity : LanguageSchema, IIdSchema<long>
    {
        public long Id { get; set; }
        public ICollection<ContentEntity> Content { get; set; }
    }
}
