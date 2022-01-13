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
    public class RoleConfiguraiton : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> b)
        {
            b.HasKey(r => r.Id);

            b.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();

            b.ToTable("Roles");

            b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            b.Property(u => u.Name).HasMaxLength(100);
            b.Property(u => u.NormalizedName).HasMaxLength(100);

            b.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

            b.HasMany<RoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

            b.HasData(new Role
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new Role
            {
                Id = 2,
                Name = "Edıtor",
                NormalizedName = "EDITOR",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            });
        }
    }
}
