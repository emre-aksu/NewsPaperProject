using AutoMapper;
using BusinessLayer.Contracts;
using DataAccessLayer.Contracts.IRepositories;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ModelLayer.Dtos.AuthorDtos;
using ModelLayer.Entities;

namespace BusinessLayer.Implementation
{
    public class AuthorManager : IAuthorManager
    {
        private readonly IAuthorRepository _authorRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorManager> _logger;
        private readonly IMemoryCache _cache;
        private readonly IValidator<AuthorPostDto> _postValidator;
        private readonly IValidator<AuthorPutDto> _putValidator;

        public AuthorManager(
            IAuthorRepository authorRepo,
            IMapper mapper,
            ILogger<AuthorManager> logger,
            IMemoryCache cache,
            IValidator<AuthorPostDto> postValidator,
            IValidator<AuthorPutDto> putValidator)
        {
            _authorRepo = authorRepo;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
            _postValidator = postValidator;
            _putValidator = putValidator;
        }

        public async Task<AuthorGetDto> GetById(int id, params string[] includeList)
        {
            string cacheKey = $"Author_{id}";
            if (_cache.TryGetValue(cacheKey, out AuthorGetDto cachedAuthor))
            {
                _logger.LogInformation($"Returning cached author with ID: {id}");
                return cachedAuthor;
            }

            _logger.LogInformation($"Fetching author with ID {id} from database.");
            Author author = await _authorRepo.GetByIdAsync(id, includeList);
            if (author == null)
            {
                _logger.LogWarning($"Author with ID {id} not found.");
                return null;
            }

            AuthorGetDto dto = _mapper.Map<AuthorGetDto>(author);
            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(10));

            return dto;
        }

        public async Task<List<AuthorGetDto>> GetAllAuthor(params string[] includeList)
        {
            const string cacheKey = "All_Authors";
            if (_cache.TryGetValue(cacheKey, out List<AuthorGetDto> cachedAuthors))
            {
                _logger.LogInformation("Returning cached authors.");
                return cachedAuthors;
            }

            _logger.LogInformation("Fetching authors from database.");
            List<Author> authors = await _authorRepo.GetAllAsync(includeList);
            List<AuthorGetDto> list = _mapper.Map<List<AuthorGetDto>>(authors);

            _cache.Set(cacheKey, list, TimeSpan.FromMinutes(10));
            return list;
        }

        public async Task AddAuthor(AuthorPostDto dto)
        {
            _logger.LogInformation("AddAuthor started.");

            var validationResult = _postValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Author>(dto);
            await _authorRepo.InsertAsync(entity);

            _logger.LogInformation("AddAuthor completed.");
        }

        public async Task UpdateAuthor(AuthorPutDto dto)
        {
            _logger.LogInformation($"Updating author with ID {dto.Id}");

            var validationResult = _putValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Author>(dto);
            await _authorRepo.UpdateAsync(entity);

            _cache.Remove($"Author_{dto.Id}");
            _logger.LogInformation($"Author with ID {dto.Id} updated.");
        }

        public async Task DeleteAuthor(int id)
        {
            _logger.LogInformation($"Deleting Author with ID: {id}");

            await _authorRepo.DeleteAsync(id);

            _cache.Remove($"Author_{id}");
            _logger.LogInformation($"Author with ID {id} deleted.");
        }

       
    }

}
