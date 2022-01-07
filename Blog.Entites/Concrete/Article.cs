﻿using Blog.Core.Entites.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entites.Concrete
{
    public class Article : EntityBase, IEntity
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Tumbnail { get; set; }
        public DateTime Date { get; set; }
        public int ViewsCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public string SeoAuthor { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTags { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
