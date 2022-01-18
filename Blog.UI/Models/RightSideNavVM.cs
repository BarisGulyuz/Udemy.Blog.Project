using Blog.Entites.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.UI.Models
{
    public class RightSideNavVM
    {
        public List<Category> Categories { get; set; }
        public List<Article> Articles { get; set; }
    }
}
