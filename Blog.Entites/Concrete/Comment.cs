using Blog.Core.Entites.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entites.Concrete
{
    public class Comment : EntityBase, IEntity
    {
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public string Text { get; set; }
    }
}
