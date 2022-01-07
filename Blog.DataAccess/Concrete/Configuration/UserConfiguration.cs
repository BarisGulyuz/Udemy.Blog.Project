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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Firstname).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Surname).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Username).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Username).IsUnique();
            builder.Property(x => x.PasswordHash).IsRequired();
            builder.Property(x => x.PasswordHash).HasColumnType("VARBINARY(500)");
            builder.Property(x => x.Firstname).HasMaxLength(500);
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ModifiedDate).IsRequired();
            builder.Property(x => x.CreatedByName).HasMaxLength(100);
            builder.Property(x => x.ModifiedByName).HasMaxLength(100);
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.Note).HasMaxLength(500);

            builder.HasOne<Role>(x => x.Role).WithMany(x => x.Users).HasForeignKey(x => x.RoleId);

            builder.HasData(new User
            {
                Id = 1,
                RoleId = 1,
                Firstname = "Barış",
                Surname = "Gülyüz",
                Username = "BG_Developer",
                Email = "bar.77@windowslive.com",
                Description = "İlk Kullanıcı",
                Picture = "none",
                PasswordHash = Encoding.ASCII.GetBytes("0192023a7bbd73250516f069df18b500"),
                IsActive = true,
                IsDeleted = false,
                CreatedByName = "InitialCreate",
                ModifiedByName = "InitialCreate",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Note = "Admin"
            });
        }
    }
}
