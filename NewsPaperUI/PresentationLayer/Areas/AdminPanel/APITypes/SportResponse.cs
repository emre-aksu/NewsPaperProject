namespace PresentationLayer.Areas.AdminPanel.APITypes
{
    public class SportResponse
    {
        public string Title { get; set; } // Başlık
        public string Content { get; set; }// İçerik
        public DateTime PublicationDate { get; set; } // Yayınlanma Tarihi
        public string Type { get; set; } // Tür
        public string CoverImage { get; set; } // Kapak Resmi
        public string SeoTitle { get; set; } // Seo Başlık
        public bool Status { get; set; } // Durum
        public int CategoryId { get; set; } // Kategori Id
        public int AutherId { get; set; } // Yazar Id
    }
}
