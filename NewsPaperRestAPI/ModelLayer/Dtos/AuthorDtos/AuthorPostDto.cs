using InfrastructorLayer.Model;
using Microsoft.AspNetCore.Http;

namespace ModelLayer.Dtos.AuthorDtos
{
    public class AuthorPostDto:InterfaceDataTransferObject
    {
        public string FirstName { get; set; }  // Ad 
        public string LastName { get; set; }   // Soyad
        public string Biography { get; set; }  // Biyografi
        public string CoverImage { get; set; } // Kapak Resmi
        public string Phone { get; set; } // Telefor 
        public string Email { get; set; } // Email  
        public IFormFile Picture { get; set; } // Resim
        public string? Content { get; set; } // içerik 
        public DateTime? CreatedDate { get; set; } // Oluşturulma tarihi
        public DateTime? PublishedDate { get; set; } // Yayınlanma tarihi
        public bool? Status { get; set; } // Durum
        public string? Title { get; set; }
        public DateTime? UpdatedDate { get; set; } // Güncellenme tarihi
        public int? ViewCount { get; set; } // Görüntülenme sayısı
        public int AnnouncementId { get; set; }
        public int AgendaId { get; set; }
    }
}
