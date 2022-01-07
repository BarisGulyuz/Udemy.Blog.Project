using Blog.Entites.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DataAccess.Concrete.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ModifiedDate).IsRequired();
            builder.Property(x => x.CreatedByName).HasMaxLength(100);
            builder.Property(x => x.ModifiedByName).HasMaxLength(100);
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.Note).HasMaxLength(500);


            builder.HasData(
                new Category
                {
                    Id = 1,
                    Title = ".NET",
                    Description = ".NET teknolojilerinin konuşulduğu kategori olması planlanmıştır",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    ModifiedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Note = ".NET Blog Kategorisi"
                },
            new Category
            {
                Id = 2,
                Title = "ANGULAR-JS",
                Description = "ANGULAR-JS teknolojilerinin konuşulduğu kategori olması planlanmıştır",
                IsActive = true,
                IsDeleted = false,
                CreatedByName = "InitialCreate",
                ModifiedByName = "InitialCreate",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Note = "ANGULAR-JS Kategorisi"

            }

            );
        }
    }
}
