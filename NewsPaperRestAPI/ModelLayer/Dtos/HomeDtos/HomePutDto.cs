using InfrastructorLayer.Model;
using Microsoft.AspNetCore.Http;

namespace ModelLayer.Dtos.HomeDtos
{
    public class HomePutDto : InterfaceDataTransferObject
    {
        public int Id { get; set; } //Id    
        public string Title { get; set; }   //Başlık
        public string Summary { get; set; } //Özet
        public string Content { get; set; } //İçerik
        public DateTime PublishedDate { get; set; } //Yayınlanma Tarihi
        public DateTime UpdatedDate { get; set; } //Güncellenme Tarihi
        public string Tags { get; set; } //Etiketler    
        public int ViewCount { get; set; } //Görüntülenme Sayısı    
        public int CommentCount { get; set; } //Yorum Sayısı
        public int LikeCount { get; set; } //Beğeni Sayısı
        public string CoverImage { get; set; } //Kapak Resmi
        public bool Status { get; set; } //Durum Aktif mi Pasif mi
        public IFormFile Picture { get; set; } //Resim
        public int CategoryId { get; set; } //Kategori Id
        public int AuthorId { get; set; } //Yazar Id
        public DateTime? CreatedDate { get; set; } // İlan oluşturulma tarihi
    }
}