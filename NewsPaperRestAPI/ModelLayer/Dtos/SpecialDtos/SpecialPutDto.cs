using InfrastructorLayer.Model;
using Microsoft.AspNetCore.Http;

namespace ModelLayer.Dtos.SpecialDtos
{
    public class SpecialPutDto:InterfaceDataTransferObject
    {
        public int Id { get; set; }
        public string PageName { get; set; } //Sayfa Adı
        public string Title { get; set; } //Başlık 
        public string Content { get; set; } //İçerik
        public string CoverImage { get; set; } //Kapak Resmi
        public IFormFile Picture { get; set; } //Resim
        public string SeoTitle { get; set; } //Seo Başlık
        public string Description { get; set; } //Seo Açıklama 
        public string SeoKeywords { get; set; } //Seo Anahtar Kelimeler
        public DateTime CreatedDate { get; set; } //Oluşturulma Tarihi
        public DateTime UpdateDate { get; set; } //Güncellenme Tarihi
        public int AuthorId { get; set; } //Yazar Id
        public int CategoryId { get; set; } //Kategori Id
        public DateTime? UpdatedDate { get; set; } // İlan güncellenme tarihi
        public bool Status { get; set; } // İlan durumu
        public DateTime? PublishedDate { get; set; }
        public int? ViewCount { get; set; }
    }
}
