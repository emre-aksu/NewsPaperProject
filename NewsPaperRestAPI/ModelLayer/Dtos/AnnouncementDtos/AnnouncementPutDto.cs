using InfrastructorLayer.Model;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLayer.Dtos.AnnouncementDtos
{
    public class AnnouncementPutDto : InterfaceDataTransferObject
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Institution { get; set; } // Kurum
        public string Content { get; set; } // içerik 
        public DateTime PublicationDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public DateTime PublishedDate { get; set; } // Yayınlanma tarihi
        public string Type { get; set; }
        public string CoverImage { get; set; }  // Kapak resmi
        public string SeoTitle { get; set; } // Seo başlık
        public string SeoDescription { get; set; } // Seo açıklama
        public DateTime CreatedDate { get; set; } // Oluşturulma tarihi
        public DateTime UpdatedDate { get; set; } // Güncellenme tarihi
        public bool Status { get; set; } // Durum

        [NotMapped]
        public IFormFile? Picture { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
    }
}