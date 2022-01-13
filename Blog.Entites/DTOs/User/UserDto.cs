using Blog.Entites.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entites.DTOs
{
    public class UserDto : DtoGetBase
    {
        public User User { get; set; }
    }
}
