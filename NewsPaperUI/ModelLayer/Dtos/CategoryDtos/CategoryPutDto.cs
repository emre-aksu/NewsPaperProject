using Microsoft.AspNetCore.Http;

namespace ModelLayer.Dtos.CategoryDtos
{
    public class CategoryPutDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Picture { get; set; }
        public string PhotoPath { get; set; }
    }
}
