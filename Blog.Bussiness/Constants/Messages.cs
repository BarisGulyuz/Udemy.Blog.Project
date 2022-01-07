using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Bussiness.Constants
{
    public static class Messages
    {
        public static string GeneralGetError = "Veri Getirilrken Bir Sorun Oluştu, Böyle Bir Veri Bulunamadı";
        public static string GeneralListError = "Liste Getirilirken Bir Hata Oluştu";
        public static string GeneralAddError = "Veri Eklenirken Bir Hata Oluştu";
        public static string GeneralUpdateError = "Veri Güncellenirken Bİr Hata Oluştu";
        public static string GeneralDeleteError = "Veri Silinirken Bir Hata Oluştu";

        public static string GeneralGetSuccess = "Veri Başarıyla Getirildi";
        public static string GeneralListSuccess = "Veriler Başarıyla Getirildi";
        public static string GeneralAddESuccess = "Veri Başarıyla Eklendi";
        public static string GeneralUpdateSuccess = "Veri Başarıyla Güncellendi";
        public static string GeneralDeleteSuccess = "Veri Başarıyla Silindi";

        public static string CategoryNotFound = "Böyle Bir Kategori Bulunamadı";
        public static string ArticleNotFound = "Böyle Bir Makale Bulunamadı";
    }
}
