using Blog.Core.Entites.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entites.Concrete
{
    public class Category : EntityBase, IEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
