using Blog.Entites.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.UI.Areas.Admin.Models
{
    public class CategoryUpdateAjaxModel
    {
        public CategoryUpdateDto CategoryUpdateDto { get; set; }
        public string CategoryPartial { get; set; }
        public CategoryDto CategoryDto { get; set; }
    }
}
