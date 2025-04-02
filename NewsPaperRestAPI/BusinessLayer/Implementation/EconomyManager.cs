using AutoMapper;
using BusinessLayer.Contracts;
using DataAccessLayer.Contracts.IRepositories;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ModelLayer.Dtos.EconomyDtos;
using ModelLayer.Entities;

namespace BusinessLayer.Implementation
{
    public class EconomyManager : IEconomyManager
    {
        private readonly IEconomyRepository _economyRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<EconomyManager> _logger;
        private readonly IMemoryCache _cache;
        private readonly IValidator<EconomyPostDto> _postValidator;
        private readonly IValidator<EconomyPutDto> _putValidator;

        public EconomyManager(
            IEconomyRepository economyRepo,
            IMapper mapper,
            ILogger<EconomyManager> logger,
            IMemoryCache cache,
            IValidator<EconomyPostDto> postValidator,
            IValidator<EconomyPutDto> putValidator)
        {
            _economyRepo = economyRepo;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
            _postValidator = postValidator;
            _putValidator = putValidator;
        }

        public async Task<EconomyGetDto> GetById(int id, params string[] includeList)
        {
            string cacheKey = $"Economy_{id}";
            if (_cache.TryGetValue(cacheKey, out EconomyGetDto cachedEconomy))
            {
                _logger.LogInformation($"Returning cached economy with ID: {id}");
                return cachedEconomy;
            }

            _logger.LogInformation($"Fetching economy with ID {id} from database.");
            Economy economy = await _economyRepo.GetByIdAsync(id, includeList);
            if (economy == null)
            {
                _logger.LogWarning($"Economy with ID {id} not found.");
                return null;
            }

            EconomyGetDto dto = _mapper.Map<EconomyGetDto>(economy);
            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(10));

            return dto;
        }

        public async Task<List<EconomyGetDto>> GetAllEconomies(params string[] includeList)
        {
            const string cacheKey = "All_Economies";
            if (_cache.TryGetValue(cacheKey, out List<EconomyGetDto> cachedEconomies))
            {
                _logger.LogInformation("Returning cached economies.");
                return cachedEconomies;
            }

            _logger.LogInformation("Fetching economies from database.");
            List<Economy> economies = await _economyRepo.GetAllAsync(includeList);
            List<EconomyGetDto> list = _mapper.Map<List<EconomyGetDto>>(economies);

            _cache.Set(cacheKey, list, TimeSpan.FromMinutes(10));
            return list;
        }

        public async Task AddEconomy(EconomyPostDto dto)
        {
            _logger.LogInformation("AddEconomy started.");

            var validationResult = _postValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Economy>(dto);
            await _economyRepo.InsertAsync(entity);

            _logger.LogInformation("AddEconomy completed.");
        }

        public async Task UpdateEconomy(EconomyPutDto dto)
        {
            _logger.LogInformation($"Updating economy with ID {dto.Id}");

            var validationResult = _putValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Economy>(dto);
            await _economyRepo.UpdateAsync(entity);

            _cache.Remove($"Economy_{dto.Id}");
            _logger.LogInformation($"Economy with ID {dto.Id} updated.");
        }

        public async Task DeleteEconomy(int id)
        {
            _logger.LogInformation($"Deleting economy with ID: {id}");

            await _economyRepo.DeleteAsync(id);

            _cache.Remove($"Economy_{id}");
            _logger.LogInformation($"Economy with ID {id} deleted.");
        }
    }

}
