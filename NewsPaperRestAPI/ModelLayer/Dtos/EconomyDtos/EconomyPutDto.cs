using InfrastructorLayer.Model;
using Microsoft.AspNetCore.Http;

namespace ModelLayer.Dtos.EconomyDtos
{
    public class EconomyPutDto:InterfaceDataTransferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Picture { get; set; }  // Resim    
        public string CoverImage { get; set; }// Kapak Resmi
        public string Content { get; set; } // İçerik 
        public DateTime PublishedDate { get; set; } // Yayınlanma Tarihi
        public DateTime UpdatedDate { get; set; } // Güncellenme Tarihi
        public string Title { get; set; } // Başlık
        public decimal ExchangeRates { get; set; } // Döviz Kurları
        public string StockIndex { get; set; } // Hisse Senetleri
        public decimal GoldPrices { get; set; } // Altın Fiyatları
        public int CategoryId { get; set; } // Kategori Id
        public int AuthorId { get; set; } // Yazar Id
        public DateTime? CreatedDate { get; set; } // İlan oluşturulma tarihi
       
        public bool Status { get; set; } // İlan durumu

        public int? ViewCount { get; set; }
    }
}
