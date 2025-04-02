using ModelLayer.Dtos.SportDtos;

namespace BusinessLayer.Contracts
{
    public interface ISportManager
    {
        Task<SportGetDto> GetById(int id, params string[] includeList);
        Task<List<SportGetDto>> GetAllSport(params string[] includeList);
        Task AddSport(SportPostDto dto);
        Task UpdateSport(SportPutDto dto);
        Task DeleteSport(int id);
    }
}
