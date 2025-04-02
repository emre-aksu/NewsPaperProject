using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Dtos.AgendaDtos
{
    public class AgendaPutDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public DateTime? PublishedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; } // İlan oluşturulma tarihi
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public string CoverImage { get; set; }
        public bool Status { get; set; }
        [NotMapped]
        public IFormFile? Picture { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
    }
}
