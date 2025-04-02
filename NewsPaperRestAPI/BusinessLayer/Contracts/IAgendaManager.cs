using ModelLayer.Dtos.AgendaDtos;

namespace BusinessLayer.Contracts
{
    public interface IAgendaManager
    {
        Task<AgendaGetDto> GetById(int id, params string[] includeList);
        Task<List<AgendaGetDto>> GetAllAgenda(params string[] includeList);
        Task AddAgenda(AgendaPostDto dto);
        Task UpdateAgenda(AgendaPutDto dto);
        Task DeleteAgenda(int id);
    }
}
