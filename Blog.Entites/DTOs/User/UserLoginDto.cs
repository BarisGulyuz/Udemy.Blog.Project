using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entites.DTOs
{
    public class UserLoginDto : DtoGetBase
    {
        private const string mail = "Mail";
        private const string bigger = "{0} {1}  Karakterden büyük olmamalıdır";
        private const string smaller = "{0} {1}  Karakterden küçük olmamalıdır";
        private const string sifre = "Şifre";
        private const string benihatırla = "Beni Hatırla";

        [DisplayName(mail)]
        [Required(ErrorMessage = "{0} Boş olmamalıdır")]
        [MaxLength(70, ErrorMessage = bigger)]
        [MinLength(2, ErrorMessage = smaller)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName(sifre)]
        [Required(ErrorMessage = "{0} Boş olmamalıdır")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName(benihatırla)]
        public bool RememberMe { get; set; }
    }
}
