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
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> b)
        {
            b.HasKey(r => new { r.UserId, r.RoleId });

            b.ToTable("UserRoles");

            b.HasData(new UserRole { UserId = 1, RoleId = 1 }, new UserRole { UserId = 2, RoleId = 2 });
        }
    }
}
