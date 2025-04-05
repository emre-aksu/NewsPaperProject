using PresentationLayer.Areas.AdminPanel.APITypes;

namespace PresentationLayer.Areas.AdminPanel.Models
{
    public class AgendaViewModel
    {
        public List<CategoryResponse> CategoryList { get; set; }
        public List<AuthorResponse> AuthorList { get; set; }
    }
}
