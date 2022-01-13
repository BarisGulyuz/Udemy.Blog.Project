using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Entites.Concrete;

namespace ProgrammersBlog.Entities
{
    public class CommentListDto
    {
        public IList<Comment> Comments { get; set; }
    }
}
