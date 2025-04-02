using AutoMapper;
using BusinessLayer.Contracts;
using DataAccessLayer.Contracts.IRepositories;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ModelLayer.Dtos.SpecialDtos;
using ModelLayer.Entities;

namespace BusinessLayer.Implementation
{
    public class SpecialManager : ISpecialManager
    {
        private readonly ISpecialRepository _specialRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<SpecialManager> _logger;
        private readonly IMemoryCache _cache;
        private readonly IValidator<SpecialPostDto> _postValidator;
        private readonly IValidator<SpecialPutDto> _putValidator;

        public SpecialManager(
            ISpecialRepository specialRepo,
            IMapper mapper,
            ILogger<SpecialManager> logger,
            IMemoryCache cache,
            IValidator<SpecialPostDto> postValidator,
            IValidator<SpecialPutDto> putValidator)
        {
            _specialRepo = specialRepo;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
            _postValidator = postValidator;
            _putValidator = putValidator;
        }

        public async Task<SpecialGetDto> GetById(int id, params string[] includeList)
        {
            string cacheKey = $"Special_{id}";
            if (_cache.TryGetValue(cacheKey, out SpecialGetDto cachedSpecial))
            {
                _logger.LogInformation($"Returning cached special with ID: {id}");
                return cachedSpecial;
            }

            _logger.LogInformation($"Fetching special with ID {id} from database.");
            Special special = await _specialRepo.GetByIdAsync(id, includeList);
            if (special == null)
            {
                _logger.LogWarning($"Special with ID {id} not found.");
                return null;
            }

            SpecialGetDto dto = _mapper.Map<SpecialGetDto>(special);
            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(10));

            return dto;
        }

        public async Task<List<SpecialGetDto>> GetAllSpecial(params string[] includeList)
        {
            const string cacheKey = "All_Specials";
            if (_cache.TryGetValue(cacheKey, out List<SpecialGetDto> cachedSpecials))
            {
                _logger.LogInformation("Returning cached specials.");
                return cachedSpecials;
            }

            _logger.LogInformation("Fetching specials from database.");
            List<Special> specials = await _specialRepo.GetAllAsync(includeList);
            List<SpecialGetDto> list = _mapper.Map<List<SpecialGetDto>>(specials);

            _cache.Set(cacheKey, list, TimeSpan.FromMinutes(10));
            return list;
        }

        public async Task AddSpecial(SpecialPostDto dto)
        {
            _logger.LogInformation("AddSpecial started.");

            var validationResult = _postValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Special>(dto);
            await _specialRepo.InsertAsync(entity);

            _logger.LogInformation("AddSpecial completed.");
        }

        public async Task UpdateSpecial(SpecialPutDto dto)
        {
            _logger.LogInformation($"Updating special with ID {dto.Id}");

            var validationResult = _putValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Special>(dto);
            await _specialRepo.UpdateAsync(entity);

            _cache.Remove($"Special_{dto.Id}");
            _logger.LogInformation($"Special with ID {dto.Id} updated.");
        }

        public async Task DeleteSpecial(int id)
        {
            _logger.LogInformation($"Deleting special with ID: {id}");

            await _specialRepo.DeleteAsync(id);

            _cache.Remove($"Special_{id}");
            _logger.LogInformation($"Special with ID {id} deleted.");
        }
    }

}
