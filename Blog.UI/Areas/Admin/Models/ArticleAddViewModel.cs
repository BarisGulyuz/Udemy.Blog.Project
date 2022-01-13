using Blog.Entites.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.UI.Areas.Admin.Models
{
    public class ArticleAddViewModel
    {
        [DisplayName("Başlık")]
        [Required(ErrorMessage = "{0} alanı boş olmamalıdır")]
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden büyük olmamalıdr")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden küçük olmamalıdr")]
        public string Title { get; set; }
        [DisplayName("İçerik")]
        [Required(ErrorMessage = "{0} alanı boş olmamalıdır")]
        [MinLength(100, ErrorMessage = "{0} {1} karakterden küçük olmamalıdr")]
        public string Content { get; set; }
        [DisplayName("Görsel")]
        [Required(ErrorMessage = "{0} alanı boş olmamalıdır")]
        public IFormFile TumbnailFile { get; set; }
        [DisplayName("Tarih")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }
        [DisplayName("Yazar Bilgisi")]
        [Required]
        public string SeoAuthor { get; set; }
        [DisplayName("Makale Açıklama")]
        [Required]
        public string SeoDescription { get; set; }
        [DisplayName("Makale Anathtar Kelimler")]
        [Required]
        public string SeoTags { get; set; }
        [Required]
        [DisplayName("Kategori")]
        public int CategoryId { get; set; }
        [DisplayName("Aktif Mi?")]
        [Required(ErrorMessage = "{0} Boş olmamalıdır")]
        public bool IsActive { get; set; }
        public List<Category> Categories { get; set; }
    }
}
