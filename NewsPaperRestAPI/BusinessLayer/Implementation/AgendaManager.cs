using AutoMapper;
using BusinessLayer.Contracts;
using DataAccessLayer.Contracts.IRepositories;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ModelLayer.Dtos.AgendaDtos;
using ModelLayer.Entities;

namespace BusinessLayer.Implementation
{
    public class AgendaManager : IAgendaManager
    {
        private readonly IAgendaRepository _agndaRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<AgendaManager> _logger;
        private readonly IMemoryCache _cache;
        private readonly IValidator<AgendaPostDto> _postValidator;
        private readonly IValidator<AgendaPutDto> _putValidator;

        public AgendaManager(
            IAgendaRepository agndaRepo,
            IMapper mapper,
            ILogger<AgendaManager> logger,
            IMemoryCache cache,
            IValidator<AgendaPostDto> postValidator,
            IValidator<AgendaPutDto> putValidator)
        {
            _agndaRepo = agndaRepo;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
            _postValidator = postValidator;
            _putValidator = putValidator;
        }

        public async Task AddAgenda(AgendaPostDto dto)
        {
            _logger.LogInformation("AddAgenda started.");

            var validationResult = _postValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Agenda>(dto);
            await _agndaRepo.InsertAsync(entity);

            _logger.LogInformation("AddAgenda completed.");
        }

        public async Task DeleteAgenda(int id)
        {
            _logger.LogInformation($"Deleting Agenda with ID: {id}");

            await _agndaRepo.DeleteAsync(id);

            _cache.Remove($"Agenda_{id}");
            _logger.LogInformation($"Agenda with ID {id} deleted.");
        }

        public async Task<List<AgendaGetDto>> GetAllAgenda(params string[] includeList)
        {
            const string cacheKey = "All_Agendas";
            if (_cache.TryGetValue(cacheKey, out List<AgendaGetDto> cachedAgendas))
            {
                _logger.LogInformation("Returning cached agendas.");
                return cachedAgendas;
            }

            _logger.LogInformation("Fetching agendas from database.");
            List<Agenda> agendas = await _agndaRepo.GetAllAsync(includeList);
            List<AgendaGetDto> list = _mapper.Map<List<AgendaGetDto>>(agendas);

            _cache.Set(cacheKey, list, TimeSpan.FromMinutes(10));
            return list;
        }

        public async Task<AgendaGetDto> GetById(int id, params string[] includeList)
        {
            string cacheKey = $"Agenda_{id}";
            if (_cache.TryGetValue(cacheKey, out AgendaGetDto cachedAgenda))
            {
                _logger.LogInformation($"Returning cached agenda with ID: {id}");
                return cachedAgenda;
            }

            _logger.LogInformation($"Fetching agenda with ID {id} from database.");
            Agenda agenda = await _agndaRepo.GetByIdAsync(id, includeList);
            if (agenda == null)
            {
                _logger.LogWarning($"Agenda with ID {id} not found.");
                return null;
            }

            AgendaGetDto dto = _mapper.Map<AgendaGetDto>(agenda);
            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(10));

            return dto;
        }

        public async Task UpdateAgenda(AgendaPutDto dto)
        {
            _logger.LogInformation($"Updating agenda with ID {dto.Id}");

            var validationResult = _putValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = _mapper.Map<Agenda>(dto);
            await _agndaRepo.UpdateAsync(entity);

            _cache.Remove($"Agenda_{dto.Id}");
            _logger.LogInformation($"Agenda with ID {dto.Id} updated.");
        }
    }

}
