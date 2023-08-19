using EasyMicroservices.ContentsMicroservice.Database.Entities;
using EasyMicroservices.Cores.Relational.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EasyMicroservices.ContentsMicroservice.Database.Contexts
{
    public class ContentContext : RelationalCoreContext
    {
        IDatabaseBuilder _builder;
        public ContentContext(IDatabaseBuilder builder)
        {
            _builder = builder;
        }

        public DbSet<CategoryEntity> Categories { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_builder != null)
                _builder.OnConfiguring(optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CategoryEntity>(model =>
            {
                model.HasKey(x => x.Id);
            });

        }
    }
}