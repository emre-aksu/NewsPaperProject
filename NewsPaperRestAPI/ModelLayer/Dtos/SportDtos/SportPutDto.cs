using InfrastructorLayer.Model;
using Microsoft.AspNetCore.Http;

namespace ModelLayer.Dtos.SportDtos
{
    public class SportPutDto:InterfaceDataTransferObject 
    {
        public int Id { get; set; } // Id   
        public string Title { get; set; } // Başlık
        public string Content { get; set; }// İçerik
        public DateTime PublicationDate { get; set; } // Yayınlanma Tarihi
        public string Type { get; set; } // Tür
        public string CoverImage { get; set; } // Kapak Resmi
        public IFormFile Picture { get; set; } // Resim
        public string SeoTitle { get; set; } // Seo Başlık
        public bool Status { get; set; } // Durum
        public int CategoryId { get; set; } // Kategori Id
        public int AuthorId { get; set; } // Yazar Id
        public DateTime? CreatedDate { get; set; } // İlan oluşturulma tarihi
        public DateTime? UpdatedDate { get; set; } // İlan güncellenme tarihi
        public DateTime? PublishedDate { get; set; }
        public int? ViewCount { get; set; }

    }
}
