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
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(x => x.Text).IsRequired().HasMaxLength(500);
                builder.Property(x => x.CreatedDate).IsRequired();
                builder.Property(x => x.ModifiedDate).IsRequired();
                builder.Property(x => x.CreatedByName).HasMaxLength(100);
                builder.Property(x => x.ModifiedByName).HasMaxLength(100);
                builder.Property(x => x.IsActive).IsRequired();
                builder.Property(x => x.IsDeleted).IsRequired();
                builder.Property(x => x.Note).HasMaxLength(500);

                builder.HasOne<Article>(x => x.Article).WithMany(x => x.Comments).HasForeignKey(x => x.ArticleId);

                //builder.HasData(new Comment
                //{
                //    Id = 1,
                //    ArticleId = 1,
                //    Text = ".NET 6 çok güzel olmuş, harika çok beğendim",
                //    IsActive = true,
                //    IsDeleted = false,
                //    CreatedByName = "InitialCreate",
                //    ModifiedByName = "InitialCreate",
                //    CreatedDate = DateTime.Now,
                //    ModifiedDate = DateTime.Now,
                //    Note = ".NET 6",
                //});
            }
        }
    }
}
