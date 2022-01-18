using Blog.Entites.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.UI.Models
{
    public class ArticlesSearchVM
    {
        public ArticleListDto ArticleListDto { get; set; }
        public string Keyword { get; set; }
    }
}
