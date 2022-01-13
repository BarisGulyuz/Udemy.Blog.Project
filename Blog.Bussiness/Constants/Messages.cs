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

        public static class Comment
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiç bir yorum bulunamadı.";
                return "Böyle bir yorum bulunamadı.";
            }

            public static string Approve(int commentId)
            {
                return $"{commentId} numaralı yorum başarıyla düzenlenmiştir.";
            }
            public static string Add(string createdByName)
            {
                return $"Sayın {createdByName}, yorumunuz başarıyla eklenmiştir.";
            }

            public static string Update(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum başarıyla güncellenmiştir.";
            }
            public static string Delete(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum başarıyla silinmiştir.";
            }
            public static string HardDelete(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum başarıyla veritabanından silinmiştir.";
            }
        }
    }
}
