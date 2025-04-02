using ModelLayer.Dtos.EconomyDtos;

namespace BusinessLayer.Contracts
{
    public interface IEconomyManager
    {
        Task<EconomyGetDto> GetById(int id, params string[] includeList);
        Task<List<EconomyGetDto>> GetAllEconomies(params string[] includeList);
        Task AddEconomy(EconomyPostDto dto);
        Task UpdateEconomy(EconomyPutDto dto);
        Task DeleteEconomy(int id);
    }
}
