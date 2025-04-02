using InfrastructorLayer.Model;

namespace ModelLayer.Dtos.AnnouncementDtos
{
    public class AnnouncementGetDto:InterfaceDataTransferObject
    {
        public string Title { get; set; }
        public string Number { get; set; }
        public string Institution { get; set; } // Kurum
        public DateTime PublishDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public string Type { get; set; }    
        public string CoverImage { get; set; }  // Kapak resmi
        public string SeoTitle { get; set; } // Seo başlık
        public string SeoDescription { get; set; } // Seo açıklama
        public DateTime CreateDate { get; set; } // Oluşturulma tarihi
        public DateTime UpdateDate { get; set; } // Güncellenme tarihi
        public bool Status { get; set; } // Durum
       // public byte[] Picture { get; set; } // Resim    
        public int CategoryId { get; set; } // Kategori Id  
        public int AthorId { get; set; } // Yazar Id
    }
}
