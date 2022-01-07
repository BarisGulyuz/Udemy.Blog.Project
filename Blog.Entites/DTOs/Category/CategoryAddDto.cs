using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entites.DTOs
{
    public class CategoryAddDto
    {
        [DisplayName("Kategori Adı")]
        [Required(ErrorMessage = "{0} Boş olmamalıdır")]
        [MaxLength(70, ErrorMessage = "{0} {1}  Karakterden büyük olmamalıdır")]
        [MinLength(2, ErrorMessage = "{0} {1}  Karakterden küçük olmamalıdır")]
        public string Title { get; set; }

        [DisplayName("Kategori Açıklaması")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")]
        [MaxLength(500, ErrorMessage = "{0} {1}  Karakterden büyük olmamalıdır")]
        [MinLength(2, ErrorMessage = "{0} {1}  Karakterden küçük olmamalıdır")]
        public string Description { get; set; }

        [DisplayName("Özel Not")]
        [MaxLength(500, ErrorMessage = "{0} {1}  Karakterden büyük olmamalıdır")]
        public string Note { get; set; }

        [DisplayName("Aktif Mi?")]
        [Required(ErrorMessage = "{0} Boş olmamalıdır")]
        public bool IsActive { get; set; }
    }
}
