using AutoMapper;
using BusinessLayer.Contracts;
using DataAccessLayer.Contracts.IRepositories;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ModelLayer.Dtos.SportDtos;
using ModelLayer.Entities;

namespace BusinessLayer.Implementation
{
    public class SportManager : ISportManager
    {
        private readonly ISportRepository _sportRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<SportManager> _logger;
        private readonly IMemoryCache _cache;
        private readonly IValidator<SportPostDto> _postValidator;
        private readonly IValidator<SportPutDto> _putValidator;

        public SportManager(
            ISportRepository sportRepo,
            IMapper mapper,
            ILogger<SportManager> logger,
            IMemoryCache cache,
            IValidator<SportPostDto> postValidator,
            IValidator<SportPutDto> putValidator)
        {
            _sportRepo = sportRepo;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
            _postValidator = postValidator;
            _putValidator = putValidator;
        }

        public async Task<SportGetDto> GetById(int id, params string[] includeList)
        {
            string cacheKey = $"Sport_{id}";
            if (_cache.TryGetValue(cacheKey, out SportGetDto cachedSport))
            {
                _logger.LogInformation($"Returning cached sport with ID: {id}");
                return cachedSport;
            }

            _logger.LogInformation($"Fetching sport with ID {id} from database.");
            Sport sport = await _sportRepo.GetByIdAsync(id, includeList);
            if (sport == null)
            {
                _logger.LogWarning($"Sport with ID {id} not found.");
                return null;
            }

            SportGetDto dto = _mapper.Map<SportGetDto>(sport);
            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(10));

            return dto;
        }

        public async Task<List<SportGetDto>> GetAllSport(params string[] includeList)
        {
            const string cacheKey = "All_Sports";
            if (_cache.TryGetValue(cacheKey, out List<SportGetDto> cachedSports))
            {
                _logger.LogInformation("Returning cached sports.");
                return cachedSports;
            }

            _logger.LogInformation("Fetching sports from database.");
            List<Sport> sports = await _sportRepo.GetAllAsync(includeList);
            List<SportGetDto> list = _mapper.Map<List<SportGetDto>>(sports);

            _cache.Set(cacheKey, list, TimeSpan.FromMinutes(10));
            return list;
        }

        public async Task AddSport(SportPostDto dto)
        {
            _logger.LogInformation("AddSport started.");

            var validationResult = _postValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Sport>(dto);
            await _sportRepo.InsertAsync(entity);

            _logger.LogInformation("AddSport completed.");
        }

        public async Task UpdateSport(SportPutDto dto)
        {
            _logger.LogInformation($"Updating sport with ID {dto.Id}");

            var validationResult = _putValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Sport>(dto);
            await _sportRepo.UpdateAsync(entity);

            _cache.Remove($"Sport_{dto.Id}");
            _logger.LogInformation($"Sport with ID {dto.Id} updated.");
        }

        public async Task DeleteSport(int id)
        {
            _logger.LogInformation($"Deleting sport with ID: {id}");

            await _sportRepo.DeleteAsync(id);

            _cache.Remove($"Sport_{id}");
            _logger.LogInformation($"Sport with ID {id} deleted.");
        }
    }

}
