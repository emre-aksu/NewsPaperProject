using BusinessLayer.Contracts;
using BusinessLayer.Implementation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ModelLayer.Dtos.AgendaDtos;

namespace PresentationsWebAPI.Controllers
{
    [Route("api/agendas")]
    [ApiController]
    public class AgendaControllers : ControllerBase
    {
        private readonly IAgendaManager _agendaManager;
        private readonly ILogger<AgendaControllers> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AgendaControllers(IAgendaManager agendaManager, ILogger<AgendaControllers> logger, IMemoryCache memoryCache, IWebHostEnvironment webHostEnvironment)
        {
            _agendaManager = agendaManager;
            _logger = logger;
            _memoryCache = memoryCache;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Agenda getiriliyor: Id = {Id}", id);
            var agendaDto = await _agendaManager.GetById(id, "Authors");
            if (agendaDto == null)
            {
                _logger.LogWarning("Agenda bulunamadı: Id = {Id}", id);
                return NotFound($"Agenda with ID {id} not found.");
            }
            return Ok(agendaDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAgenda()
        {
            _logger.LogInformation("Tüm agendalar getiriliyor.");
            var agendas = await _agendaManager.GetAllAgenda("Authors");
            if (agendas == null || agendas.Count == 0)
            {
                _logger.LogWarning("Hiç agenda bulunamadı.");
                return Ok(new List<AgendaGetDto>());
            }
            return Ok(agendas);
        }

        [HttpPost]
        public async Task<IActionResult> AddAgenda([FromForm] AgendaPostDto dto)
        {
            _logger.LogInformation("Yeni bir agenda ekleniyor: {@Agenda}", dto);

            if (dto.Picture != null)
            {
                try
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileExtension = Path.GetExtension(dto.Picture.FileName).ToLower();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest("Geçersiz dosya formatı. Sadece .jpg, .jpeg, .png ve .gif desteklenmektedir.");
                    }

                    var fileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.Picture.CopyToAsync(stream);
                    }

                    dto.CoverImage = $"/uploads/{fileName}";
                }
                catch (Exception ex)
                {
                    _logger.LogError("Dosya yükleme hatası: {Message}", ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dosya yüklenirken bir hata oluştu.");
                }
            }

            await _agendaManager.AddAgenda(dto);
            _memoryCache.Remove("AgendaList");

            return StatusCode(StatusCodes.Status201Created, new { message = "Agenda başarıyla eklendi!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAgendas(int id, [FromForm] AgendaPutDto dto)
        {
            _logger.LogInformation("Agenda güncelleniyor: Id = {Id}, Yeni Değerler = {@Dto}", id, dto);

            var existingAgenda = await _agendaManager.GetById(id);
            if (existingAgenda == null)
            {
                _logger.LogWarning("Güncellenmek istenen agenda bulunamadı: Id = {Id}", id);
                return NotFound($"Agenda with ID {id} not found.");
            }

            if (dto.Picture != null)
            {
                try
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileExtension = Path.GetExtension(dto.Picture.FileName).ToLower();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest("Geçersiz dosya formatı. Sadece .jpg, .jpeg, .png ve .gif desteklenmektedir.");
                    }

                    // **Eski resmi silme işlemi**
                    if (!string.IsNullOrEmpty(existingAgenda.CoverImage))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingAgenda.CoverImage.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var fileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.Picture.CopyToAsync(stream);
                    }

                    dto.CoverImage = $"/uploads/{fileName}";
                }
                catch (Exception ex)
                {
                    _logger.LogError("Dosya yükleme hatası: {Message}", ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dosya yüklenirken bir hata oluştu.");
                }
            }
            else
            {
                // Eğer yeni resim yüklenmediyse, eski resmi koru
                dto.CoverImage = existingAgenda.CoverImage;
            }

            await _agendaManager.UpdateAgenda(dto);
            _memoryCache.Remove("AgendaList");
            return Ok(new { message = "Agenda başarıyla güncellendi!" });
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgenda(int id)
        {
            _logger.LogWarning("Agenda siliniyor: Id = {Id}", id);
            await _agendaManager.DeleteAgenda(id);
            _memoryCache.Remove("AgendaList");
            return Ok(new { message = "Agenda başarıyla silindi!" });
        }
    }
}
