using Blog.Entites.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.UI.Areas.Admin.Models
{
    public class UserRoleViewModel
    {
        public User User { get; set; }
        public IList<string> Roles { get; set; }
    }
}
