using ModelLayer.Dtos.AnnouncementDtos;

namespace BusinessLayer.Contracts
{
    public interface IAnnouncementManager
    {
        Task<AnnouncementGetDto> GetById(int id, params string[] includeList);
        Task<List<AnnouncementGetDto>> GetAllAnnouncement(params string[] includeList);
        Task AddAnnouncement(AnnouncementPostDto dto);
        Task UpdateAnnouncement(AnnouncementPutDto dto);
        Task DeleteAnnouncement(int id);
    }
}

