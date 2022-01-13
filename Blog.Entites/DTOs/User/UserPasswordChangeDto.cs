using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entites.DTOs
{
    public class UserPasswordChangeDto
    {

        private const string sifre = "Mevcut Şifre";
        private const string yenisifre = "Yeni Şifre";
        private const string yenisifretekrar = "Yeni Şifre Tekrar";
        private const string sifreleruyusmuyor = "Şifreler Uyuşmuyor";

        [DisplayName(sifre)]
        [Required(ErrorMessage = "{0} Boş olmamalıdır")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DisplayName(yenisifre)]
        [Required(ErrorMessage = "{0} Boş olmamalıdır")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = sifreleruyusmuyor)]
        [DisplayName(yenisifretekrar)]
        [Required(ErrorMessage = "{0} Boş olmamalıdır")]
        [DataType(DataType.Password)]
        public string NewPasswordAgain { get; set; }
    }
}
