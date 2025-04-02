using AutoMapper;
using BusinessLayer.Contracts;
using DataAccessLayer.Contracts.IRepositories;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ModelLayer.Dtos.CategoryDtos;
using ModelLayer.Entities;

namespace BusinessLayer.Implementation
{
    public class CategoryManager : ICategoryManager
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryManager> _logger;
        private readonly IMemoryCache _cache;
        private readonly IValidator<CategoryPostDto> _postValidator;
        private readonly IValidator<CategoryPutDto> _putValidator;

        public CategoryManager(
            ICategoryRepository categoryRepo,
            IMapper mapper,
            ILogger<CategoryManager> logger,
            IMemoryCache cache,
            IValidator<CategoryPostDto> postValidator,
            IValidator<CategoryPutDto> putValidator)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
            _postValidator = postValidator;
            _putValidator = putValidator;
        }

        public async Task<CategoryGetDto> GetById(int id, params string[] includeList)
        {
            string cacheKey = $"Category_{id}";
            if (_cache.TryGetValue(cacheKey, out CategoryGetDto cachedCategory))
            {
                _logger.LogInformation($"Returning cached category with ID: {id}");
                return cachedCategory;
            }

            _logger.LogInformation($"Fetching category with ID {id} from database.");
            Category category = await _categoryRepo.GetByIdAsync(id, includeList);
            if (category == null)
            {
                _logger.LogWarning($"Category with ID {id} not found.");
                return null;
            }

            CategoryGetDto dto = _mapper.Map<CategoryGetDto>(category);
            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(10));

            return dto;
        }

        public async Task<List<CategoryGetDto>> GetAllCategories(params string[] includeList)
        {
            const string cacheKey = "All_Categories";
            if (_cache.TryGetValue(cacheKey, out List<CategoryGetDto> cachedCategories))
            {
                _logger.LogInformation("Returning cached categories.");
                return cachedCategories;
            }

            _logger.LogInformation("Fetching categories from database.");
            List<Category> categories = await _categoryRepo.GetAllAsync(includeList);
            List<CategoryGetDto> list = _mapper.Map<List<CategoryGetDto>>(categories);

            _cache.Set(cacheKey, list, TimeSpan.FromMinutes(10));
            return list;
        }

        public async Task AddCategory(CategoryPostDto dto)
        {
            _logger.LogInformation("AddCategory started.");

            var validationResult = _postValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Category>(dto);
            await _categoryRepo.InsertAsync(entity);

            _logger.LogInformation("AddCategory completed.");
        }

        public async Task UpdateCategory(CategoryPutDto dto)
        {
            _logger.LogInformation($"Updating category with ID {dto.Id}");

            var validationResult = _putValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Category>(dto);
            await _categoryRepo.UpdateAsync(entity);

            _cache.Remove($"Category_{dto.Id}");
            _logger.LogInformation($"Category with ID {dto.Id} updated.");
        }

        public async Task DeleteCategory(int id)
        {
            _logger.LogInformation($"Deleting category with ID: {id}");

            await _categoryRepo.DeleteAsync(id);

            _cache.Remove($"Category_{id}");
            _logger.LogInformation($"Category with ID {id} deleted.");
        }
    }

}
