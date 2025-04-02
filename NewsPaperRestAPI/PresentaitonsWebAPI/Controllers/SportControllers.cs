using BusinessLayer.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ModelLayer.Dtos.SportDtos;

namespace PresentationsWebAPI.Controllers
{
    [Route("api/sports")]
    [ApiController]
    public class SportController : ControllerBase
    {
        private readonly ISportManager _sportManager;
        private readonly ILogger<SportController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SportController(ISportManager sportManager, ILogger<SportController> logger, IMemoryCache memoryCache, IWebHostEnvironment webHostEnvironment)
        {
            _sportManager = sportManager;
            _logger = logger;
            _memoryCache = memoryCache;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Spor içeriği getiriliyor: Id = {Id}", id);
            var sportDto = await _sportManager.GetById(id, "Authors");
            if (sportDto == null)
            {
                _logger.LogWarning("Spor içeriği bulunamadı: Id = {Id}", id);
                return NotFound($"Sport with ID {id} not found.");
            }
            return Ok(sportDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSports()
        {
            _logger.LogInformation("Tüm spor içerikleri getiriliyor.");
            var sports = await _sportManager.GetAllSport("Authors");
            if (sports == null || sports.Count == 0)
            {
                _logger.LogWarning("Hiç spor içeriği bulunamadı.");
                return Ok(new List<SportGetDto>());
            }
            return Ok(sports);
        }

        [HttpPost]
        public async Task<IActionResult> AddSport([FromForm] SportPostDto dto)
        {
            _logger.LogInformation("Yeni spor içeriği ekleniyor: {@Sport}", dto);

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

            await _sportManager.AddSport(dto);
            _memoryCache.Remove("SportList");

            return StatusCode(StatusCodes.Status201Created, new { message = "Spor içeriği başarıyla eklendi!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSport(int id, [FromForm] SportPutDto dto)
        {
            _logger.LogInformation("Spor içeriği güncelleniyor: Id = {Id}, Yeni Değerler = {@Dto}", id, dto);

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

            await _sportManager.UpdateSport(dto);
            _memoryCache.Remove("SportList");
            return Ok(new { message = "Spor içeriği başarıyla güncellendi!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSport(int id)
        {
            _logger.LogWarning("Spor içeriği siliniyor: Id = {Id}", id);
            await _sportManager.DeleteSport(id);
            _memoryCache.Remove("SportList");
            return Ok(new { message = "Spor içeriği başarıyla silindi!" });
        }
    }
}
