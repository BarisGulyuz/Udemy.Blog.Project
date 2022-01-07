using Blog.Entites.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entites.DTOs
{
    public class CategoryDto : DtoGetBase
    {
        public Category Category { get; set; }
    }
}
