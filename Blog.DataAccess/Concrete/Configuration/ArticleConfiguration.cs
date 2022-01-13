using Blog.Entites.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Blog.DataAccess.Concrete.Configuration
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.Content).HasColumnType("NVARCHAR(MAX)");
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.SeoAuthor).IsRequired();
            builder.Property(x => x.SeoDescription).HasMaxLength(100);
            builder.Property(x => x.SeoTags).HasMaxLength(100);
            builder.Property(x => x.ViewsCount).IsRequired();
            builder.Property(x => x.CommentCount).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Tumbnail).IsRequired();
            builder.Property(x => x.CreatedByName).HasMaxLength(50);
            builder.Property(x => x.ModifiedByName).HasMaxLength(50);
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.Note).HasMaxLength(500);

            builder.HasOne<Category>(x => x.Category).WithMany(x => x.Articles).HasForeignKey(x => x.CategoryId);
            builder.HasOne<User>(x => x.User).WithMany(x => x.Articles).HasForeignKey(x => x.UserId);


            //string content = ".Net 6 sürümü ile hedeflenen en temel feature .Net 5 ile başlayan birleştirme senaryosunun son kısımlarını sunmaktır.";
            //builder.HasData(new Article
            //{
            //    Id = 1,
            //    CategoryId = 1,
            //    UserId = 1,
            //    Title = ".NET 6 Yenilikleri",
            //    Content = content,
            //    Tumbnail = "Default.jpg",
            //    SeoDescription = ".NET 6 Yenilikleri",
            //    SeoTags = "c#, .NEt, .NET 6",
            //    SeoAuthor = "Barış Gülyüz",
            //    Date = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    CreatedByName = "InitialCreate",
            //    ModifiedByName = "InitialCreate",
            //    CreatedDate = DateTime.Now,
            //    ModifiedDate = DateTime.Now,
            //    Note = ".NET 6",
            //    ViewsCount = 100,
            //    CommentCount = 1

            //});
        }
    }
}
