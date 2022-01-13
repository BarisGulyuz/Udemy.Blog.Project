using Blog.Entites.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entites.DTOs
{
    public class ArticleUpdateDto
    {
        [Required]
        public int Id { get; set; }
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
        public string Tumbnail { get; set; }
        [DisplayName("Tarih")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }
        //public int ViewsCount { get; set; }
        //public int CommentCount { get; set; }
        [DisplayName("SEO Yazar Bilgisi")]
        [Required]
        public string SeoAuthor { get; set; }
        [DisplayName("SEO Açıklama Bilgisi")]
        [Required]
        public string SeoDescription { get; set; }
        [DisplayName("SEO Tag Bilgisi")]
        [Required]
        public string SeoTags { get; set; }
        [Required]
        [DisplayName("Kategori")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [DisplayName("Aktif Mi?")]
        [Required(ErrorMessage = "{0} Boş olmamalıdır")]
        public bool IsActive { get; set; }
        [DisplayName("Pasif Mi?")]
        public bool IsDeleted { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
