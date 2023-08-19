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
        public DbSet<ContentEntity> Contents { get; set; }
        public DbSet<LanguageEntity> Languages { get; set; }


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
            modelBuilder.Entity<ContentEntity>(model =>
            {
                model.HasKey(x => x.Id);

                model.HasOne(x => x.Category)
                .WithMany(x => x.Content)
                .HasForeignKey(x => x.CategoryId);

                model.HasOne(x => x.Language)
                .WithMany(x => x.Content)
                .HasForeignKey(x => x.LanguageId);
            });
            modelBuilder.Entity<LanguageEntity>(model =>
            {
                model.HasKey(x => x.Id);
            });
        }
    }
}