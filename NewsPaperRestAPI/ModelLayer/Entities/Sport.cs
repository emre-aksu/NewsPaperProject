using InfrastructorLayer.Model;

namespace ModelLayer.Entities
{
    public class Sport : BaseRecord<int>
    {
        public string Type { get; set; }
        public string SeoTitle { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
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
