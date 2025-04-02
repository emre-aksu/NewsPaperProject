using InfrastructorLayer.Model;

namespace ModelLayer.Entities
{
    public class Author:BaseRecord<int>
    {
        public string FirstName { get; set; }     
        public string LastName { get; set; }
        public string Biography { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int AnnouncementId { get; set; }
        public int AgendaId { get; set; }
        public Announcement? Announcements { get; set; }
        public Agenda? Agendas { get; set; }
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
