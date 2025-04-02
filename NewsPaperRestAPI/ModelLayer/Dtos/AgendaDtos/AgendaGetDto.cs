using InfrastructorLayer.Model;

namespace ModelLayer.Dtos.AgendaDtos
{
    public class AgendaGetDto:InterfaceDataTransferObject
    {
        public string Summary { get; set; }
        public int LikeCount { get; set; }
        public string CoverImage { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; } 
        public DateTime UpdatedDate { get; set; } 
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
    }
}
