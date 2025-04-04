namespace PresentationLayer.Areas.AdminPanel.APITypes
{
    public class CategoryPutRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Picture { get; set; }
        public string PhotoPath { get; set; }
    }
}
