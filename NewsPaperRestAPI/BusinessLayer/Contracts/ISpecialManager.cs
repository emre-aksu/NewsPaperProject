using ModelLayer.Dtos.SpecialDtos;

namespace BusinessLayer.Contracts
{
    public interface ISpecialManager
    {
        Task<SpecialGetDto> GetById(int id, params string[] includeList);
        Task<List<SpecialGetDto>> GetAllSpecial(params string[] includeList);
        Task AddSpecial(SpecialPostDto dto);
        Task UpdateSpecial(SpecialPutDto dto);
        Task DeleteSpecial(int id);
    }
}
