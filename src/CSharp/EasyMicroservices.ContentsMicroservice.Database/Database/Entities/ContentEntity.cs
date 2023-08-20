using EasyMicroservices.ContentsMicroservice.Database.Schemas;
using EasyMicroservices.Cores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.ContentsMicroservice.Database.Entities
{
    public class ContentEntity : ContentSchema, IIdSchema<long>
    {
        public long Id { get; set; }
        public long LanguageId { get; set; }
        public LanguageEntity Language { get; set; }
        public long CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
    }
}
