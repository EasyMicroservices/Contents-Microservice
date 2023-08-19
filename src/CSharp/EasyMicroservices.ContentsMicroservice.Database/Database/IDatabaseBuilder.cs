using Microsoft.EntityFrameworkCore;

namespace EasyMicroservices.ContentsMicroservice.Database
{
    public interface IDatabaseBuilder
    {
        void OnConfiguring(DbContextOptionsBuilder optionsBuilder);
    }
}
