using Blog.Core.Utilities.Results;
using Blog.Entites.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entites.DTOs
{
    public class ArticleListDto : DtoGetBase
    {
        public List<Article> Articles { get; set; }
        public int? CategoryId { get; set; }
        public virtual int CurrentPage { get; set; } = 1;
        public virtual int PageSize { get; set; } = 5;
        public virtual int TotalCount { get; set; }
        public virtual int TotalPages => (int)(Math.Ceiling(decimal.Divide(TotalCount, PageSize)));
        public virtual bool ShowPrevious => CurrentPage > 1;
        public virtual bool ShowNext => CurrentPage < TotalPages;
        public virtual bool isAscending { get; set; } = false;

        //base class'a tasinabilir
    }
}
