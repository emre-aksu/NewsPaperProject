using InfrastructorLayer.Model;

namespace ModelLayer.Entities
{
    public class Announcement:BaseRecord<int>
    {
     
        public string Institution { get; set; } // Kurum adı
        public string Content { get; set; } // İlan içeriği
        public DateTime PublicationDate { get; set; } // İlan yayın tarihi
        public DateTime DeadlineDate { get; set; } // İlan son başvuru tarihi
        public string Type { get; set; } // İlan türü
         public string SeoTitle { get; set; } // Seo başlık (Dolar/TL 2024: Güncel Kur Analizi ve Yorumlar vs için)
        public string SeoDescription { get; set; } // Seo açıklama
       
        public List<Author> Authors { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public Category Categories { get; set; }
        public string CoverImage { get; set; }
        public byte[] Picture { get; set; }
        public string? Title { get; set; }
        public DateTime? CreatedDate { get; set; } // İlan oluşturulma tarihi
        public DateTime? UpdatedDate { get; set; } // İlan güncellenme tarihi
        public bool Status { get; set; } // İlan durumu
        public DateTime? PublishedDate { get; set; }
        public int? ViewCount { get; set; }




    }
}
