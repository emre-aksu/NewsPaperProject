using ModelLayer.Dtos.AuthorDtos;

namespace BusinessLayer.Contracts
{
    public interface IAuthorManager
    {
        Task<AuthorGetDto> GetById(int id, params string[] includeList);
        Task<List<AuthorGetDto>> GetAllAuthor(params string[] includeList); 
        Task AddAuthor(AuthorPostDto dto);
        Task UpdateAuthor(AuthorPutDto dto);
        Task DeleteAuthor(int id);
    }
}
