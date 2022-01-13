using Blog.Entites.Concrete;
using Microsoft.AspNetCore.Identity;
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
        public void Configure(EntityTypeBuilder<User> b)
        {
            b.HasKey(u => u.Id);

            b.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
            b.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");

            b.ToTable("Users");

            b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            b.Property(u => u.UserName).HasMaxLength(50);
            b.Property(u => u.NormalizedUserName).HasMaxLength(50);
            b.Property(u => u.Email).HasMaxLength(100);
            b.Property(u => u.NormalizedEmail).HasMaxLength(100);


            b.HasMany<UserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

            b.HasMany<UserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

            b.HasMany<UserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

            b.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();


            var adminUser = new User
            {
                Id = 1,
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                PhoneNumber = "+905555556678",
                Picture = "default.png",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            adminUser.PasswordHash = CreatePasswordHash(adminUser, adminUser.UserName);
            var editorUser = new User
            {
                Id = 2,
                UserName = "Editör",
                NormalizedUserName = "EDITOR",
                Email = "editor@gmail.com",
                NormalizedEmail = "EDITOR@GMAIL.COM",
                PhoneNumber = "+905555556678",
                Picture = "default.png",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            editorUser.PasswordHash = CreatePasswordHash(editorUser, editorUser.UserName);

            b.HasData(adminUser, editorUser);
        }

        private string CreatePasswordHash(User user, string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            return passwordHasher.HashPassword(user, password);
        }
    }
}
