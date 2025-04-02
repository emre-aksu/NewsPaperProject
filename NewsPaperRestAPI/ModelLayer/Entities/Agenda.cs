using InfrastructorLayer.Model;
using Microsoft.AspNetCore.Http;

namespace ModelLayer.Entities
{
    public class Agenda:BaseRecord<int>
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public string CoverImage { get; set; }
        public bool Status { get; set; }
        public IFormFile Picture { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public List<Author> Authors { get; set; }
        public Category Categories { get; set; }
       
        public DateTime? CreatedDate { get; set; } // İlan oluşturulma tarihi
        public DateTime? UpdatedDate { get; set; } // İlan güncellenme tarihi
       
        public DateTime? PublishedDate { get; set; }
        

    }
}
