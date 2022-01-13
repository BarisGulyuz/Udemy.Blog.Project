using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entites.DTOs
{
    public class UserUpdateDto : DtoGetBase
    {
        private const string mail = "Mail";
        private const string ad = "Kullanıcı Adı";
        private const string bigger = "{0} {1}  Karakterden büyük olmamalıdır";
        private const string smaller = "{0} {1}  Karakterden küçük olmamalıdır";
        private const string sifre = "Şifre";
        private const string telefon = "Telefon Numarası";
        private const string görsel = "Görsel";
        private const string görselEkle = "Görsel Ekle";

        public int Id { get; set; }
        [DisplayName(ad)]
        [Required(ErrorMessage = "{0} Boş olmamalıdır")]
        [MaxLength(70, ErrorMessage = bigger)]
        [MinLength(2, ErrorMessage = smaller)]
        public string UserName { get; set; }
        [DisplayName(mail)]
        [Required(ErrorMessage = "{0} Boş olmamalıdır")]
        [MaxLength(70, ErrorMessage = bigger)]
        [MinLength(2, ErrorMessage = smaller)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName(sifre)]
        //[Required(ErrorMessage = "{0} Boş olmamalıdır")]
        [MaxLength(70, ErrorMessage = bigger)]
        [MinLength(8, ErrorMessage = smaller)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName(telefon)]
        [Required(ErrorMessage = "{0} Boş olmamalıdır")]
        [MaxLength(13, ErrorMessage = bigger)]
        [MinLength(13, ErrorMessage = smaller)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Upload)]
        [DisplayName(görselEkle)]
        public IFormFile PictureFile { get; set; }
        [DisplayName(görsel)]
        public string Picture { get; set; }
    }
}
