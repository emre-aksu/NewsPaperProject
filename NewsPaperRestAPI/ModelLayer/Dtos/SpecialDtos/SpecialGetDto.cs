using InfrastructorLayer.Model;

namespace ModelLayer.Dtos.SpecialDtos
{
    public class SpecialGetDto : InterfaceDataTransferObject
    {
        public string PageName { get; set; } //Sayfa Adı
        public string Title { get; set; } //Başlık 
        public string Content { get; set; } //İçerik
        public string CoverImage { get; set; } //Kapak Resmi
        public string SeoTitle { get; set; } //Seo Başlık
        public string SeoDescription { get; set; } //Seo Açıklama
        public string SeoKeywords { get; set; } //Seo Anahtar Kelimeler
        public DateTime CreatedDate { get; set; } //Oluşturulma Tarihi
        public DateTime UpdateDate { get; set; } //Güncellenme Tarihi
        public int AuthorId { get; set; } //Yazar Id
        public int CategoryId { get; set; } //Kategori Id

    }
}
