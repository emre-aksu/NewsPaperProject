using ModelLayer.Dtos.CategoryDtos;

namespace BusinessLayer.Contracts
{
    public interface ICategoryManager
    {
        Task<CategoryGetDto> GetById(int id, params string[] includeList);
        Task<List<CategoryGetDto>> GetAllCategories(params string[] includeList);
        Task AddCategory(CategoryPostDto dto);
        Task UpdateCategory(CategoryPutDto dto);
        Task DeleteCategory(int id);
    }
}
