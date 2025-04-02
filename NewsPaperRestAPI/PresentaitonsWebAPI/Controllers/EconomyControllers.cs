using BusinessLayer.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ModelLayer.Dtos.EconomyDtos;

namespace PresentationsWebAPI.Controllers
{
    [Route("api/economies")]
    [ApiController]
    public class EconomyController : ControllerBase
    {
        private readonly IEconomyManager _economyManager;
        private readonly ILogger<EconomyController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EconomyController(IEconomyManager economyManager, ILogger<EconomyController> logger, IMemoryCache memoryCache, IWebHostEnvironment webHostEnvironment)
        {
            _economyManager = economyManager;
            _logger = logger;
            _memoryCache = memoryCache;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Ekonomi getiriliyor: Id = {Id}", id);
            var economyDto = await _economyManager.GetById(id, "Authors");
            if (economyDto == null)
            {
                _logger.LogWarning("Ekonomi bulunamadı: Id = {Id}", id);
                return NotFound($"Economy with ID {id} not found.");
            }
            return Ok(economyDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEconomies()
        {
            _logger.LogInformation("Tüm ekonomiler getiriliyor.");
            var economies = await _economyManager.GetAllEconomies("Authors");
            if (economies == null || economies.Count == 0)
            {
                _logger.LogWarning("Hiç ekonomi bulunamadı.");
                return Ok(new List<EconomyGetDto>());
            }
            return Ok(economies);
        }

        [HttpPost]
        public async Task<IActionResult> AddEconomy([FromForm] EconomyPostDto dto)
        {
            _logger.LogInformation("Yeni bir ekonomi ekleniyor: {@Economy}", dto);

            if (dto.Picture != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"{Guid.NewGuid()}_{dto.Picture.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Picture.CopyToAsync(stream);
                }

                dto.CoverImage = $"/uploads/{fileName}";
            }

            await _economyManager.AddEconomy(dto);
            _memoryCache.Remove("EconomyList");

            return StatusCode(StatusCodes.Status201Created, new { message = "Ekonomi başarıyla eklendi!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEconomy(int id, [FromForm] EconomyPutDto dto)
        {
            _logger.LogInformation("Ekonomi güncelleniyor: Id = {Id}, Yeni Değerler = {@Dto}", id, dto);

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

            dto.UpdatedDate = DateTime.UtcNow;
            await _economyManager.UpdateEconomy(dto);
            _memoryCache.Remove("EconomyList");

            return Ok(new { message = "Ekonomi başarıyla güncellendi!" });
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEconomy(int id)
        {
            _logger.LogWarning("Ekonomi siliniyor: Id = {Id}", id);
            await _economyManager.DeleteEconomy(id);
            _memoryCache.Remove("EconomyList");
            return Ok(new { message = "Ekonomi başarıyla silindi!" });
        }
    }
}
