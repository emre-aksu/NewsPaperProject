using AutoMapper;
using BusinessLayer.Contracts;
using DataAccessLayer.Contracts.IRepositories;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ModelLayer.Dtos.AnnouncementDtos;
using ModelLayer.Entities;

namespace BusinessLayer.Implementation
{


    public class AnnouncementManager : IAnnouncementManager
    {
        private readonly IAnnouncementRepository _announcementRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<AnnouncementManager> _logger;
        private readonly IMemoryCache _cache;
        private readonly IValidator<AnnouncementPostDto> _postValidator;
        private readonly IValidator<AnnouncementPutDto> _putValidator;

        public AnnouncementManager(
            IAnnouncementRepository announcementRepo,
            IMapper mapper,
            ILogger<AnnouncementManager> logger,
            IMemoryCache cache,
            IValidator<AnnouncementPostDto> postValidator,
            IValidator<AnnouncementPutDto> putValidator)
        {
            _announcementRepo = announcementRepo;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
            _postValidator = postValidator;
            _putValidator = putValidator;
        }

        public async Task AddAnnouncement(AnnouncementPostDto dto)
        {

            _logger.LogInformation("AddAnnouncement started.");

            var validationResult = _postValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        
            var entity = _mapper.Map<Announcement>(dto);
            await _announcementRepo.InsertAsync(entity);

            _logger.LogInformation("AddAnnouncement completed.");
        }

        public async Task DeleteAnnouncement(int id)
        {
            _logger.LogInformation($"Deleting Announcement with ID: {id}");

            await _announcementRepo.DeleteAsync(id);

            _cache.Remove($"Announcement_{id}");
            _logger.LogInformation($"Announcement with ID {id} deleted.");
        }

        public async Task<List<AnnouncementGetDto>> GetAllAnnouncement(params string[] includeList)
        {
            const string cacheKey = "All_Announcements";
            if (_cache.TryGetValue(cacheKey, out List<AnnouncementGetDto> cachedAnnouncements))
            {
                _logger.LogInformation("Returning cached announcements.");
                return cachedAnnouncements;
            }

            _logger.LogInformation("Fetching announcements from database.");
            List<Announcement> announcements = await _announcementRepo.GetAllAsync(includeList);
            List<AnnouncementGetDto> list = _mapper.Map<List<AnnouncementGetDto>>(announcements);

            _cache.Set(cacheKey, list, TimeSpan.FromMinutes(10));
            return list;
        }

        public async Task<AnnouncementGetDto> GetById(int id, params string[] includeList)
        {
            string cacheKey = $"Announcement_{id}";
            if (_cache.TryGetValue(cacheKey, out AnnouncementGetDto cachedAnnouncement))
            {
                _logger.LogInformation($"Returning cached announcement with ID: {id}");
                return cachedAnnouncement;
            }

            _logger.LogInformation($"Fetching announcement with ID {id} from database.");
            Announcement announcement = await _announcementRepo.GetByIdAsync(id, includeList);
            if (announcement == null)
            {
                _logger.LogWarning($"Announcement with ID {id} not found.");
                return null;
            }

            AnnouncementGetDto dto = _mapper.Map<AnnouncementGetDto>(announcement);
            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(10));

            return dto;
        }

        public async Task UpdateAnnouncement(AnnouncementPutDto dto)
        {
            _logger.LogInformation($"Updating announcement with ID {dto.Id}");

            var validationResult = _putValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Announcement>(dto);
            await _announcementRepo.UpdateAsync(entity);

            _cache.Remove($"Announcement_{dto.Id}");
            _logger.LogInformation($"Announcement with ID {dto.Id} updated.");
        }
    }

}
