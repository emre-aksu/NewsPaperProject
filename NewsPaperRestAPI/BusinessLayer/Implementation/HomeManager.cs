using AutoMapper;
using BusinessLayer.Contracts;
using DataAccessLayer.Contracts.IRepositories;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ModelLayer.Dtos.HomeDtos;
using ModelLayer.Entities;

namespace BusinessLayer.Implementation
{
    public class HomeManager : IHomeManager
    {
        private readonly IHomeRepository _homeRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeManager> _logger;
        private readonly IMemoryCache _cache;
        private readonly IValidator<HomePostDto> _postValidator;
        private readonly IValidator<HomePutDto> _putValidator;

        public HomeManager(
            IHomeRepository homeRepo,
            IMapper mapper,
            ILogger<HomeManager> logger,
            IMemoryCache cache,
            IValidator<HomePostDto> postValidator,
            IValidator<HomePutDto> putValidator)
        {
            _homeRepo = homeRepo;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
            _postValidator = postValidator;
            _putValidator = putValidator;
        }

        public async Task<HomeGetDto> GetById(int id, params string[] includeList)
        {
            string cacheKey = $"Home_{id}";
            if (_cache.TryGetValue(cacheKey, out HomeGetDto cachedHome))
            {
                _logger.LogInformation($"Returning cached home with ID: {id}");
                return cachedHome;
            }

            _logger.LogInformation($"Fetching home with ID {id} from database.");
            Home home = await _homeRepo.GetByIdAsync(id, includeList);
            if (home == null)
            {
                _logger.LogWarning($"Home with ID {id} not found.");
                return null;
            }

            HomeGetDto dto = _mapper.Map<HomeGetDto>(home);
            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(10));

            return dto;
        }

        public async Task<List<HomeGetDto>> GetAllHome(params string[] includeList)
        {
            const string cacheKey = "All_Homes";
            if (_cache.TryGetValue(cacheKey, out List<HomeGetDto> cachedHomes))
            {
                _logger.LogInformation("Returning cached homes.");
                return cachedHomes;
            }

            _logger.LogInformation("Fetching homes from database.");
            List<Home> homes = await _homeRepo.GetAllAsync(includeList);
            List<HomeGetDto> list = _mapper.Map<List<HomeGetDto>>(homes);

            _cache.Set(cacheKey, list, TimeSpan.FromMinutes(10));
            return list;
        }

        public async Task AddHome(HomePostDto dto)
        {
            _logger.LogInformation("AddHome started.");

            var validationResult = _postValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Home>(dto);
            await _homeRepo.InsertAsync(entity);

            _logger.LogInformation("AddHome completed.");
        }

        public async Task UpdateHome(HomePutDto dto)
        {
            _logger.LogInformation($"Updating home with ID {dto.Id}");

            var validationResult = _putValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Home>(dto);
            await _homeRepo.UpdateAsync(entity);

            _cache.Remove($"Home_{dto.Id}");
            _logger.LogInformation($"Home with ID {dto.Id} updated.");
        }

        public async Task DeleteHome(int id)
        {
            _logger.LogInformation($"Deleting home with ID: {id}");

            await _homeRepo.DeleteAsync(id);

            _cache.Remove($"Home_{id}");
            _logger.LogInformation($"Home with ID {id} deleted.");
        }
    }

}
