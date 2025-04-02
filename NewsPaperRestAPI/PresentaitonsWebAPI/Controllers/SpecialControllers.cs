using BusinessLayer.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ModelLayer.Dtos.SpecialDtos;

namespace PresentationsWebAPI.Controllers
{
    [Route("api/specials")]
    [ApiController]
    public class SpecialController : ControllerBase
    {
        private readonly ISpecialManager _specialManager;
        private readonly ILogger<SpecialController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SpecialController(ISpecialManager specialManager, ILogger<SpecialController> logger, IMemoryCache memoryCache, IWebHostEnvironment webHostEnvironment)
        {
            _specialManager = specialManager;
            _logger = logger;
            _memoryCache = memoryCache;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Özel içerik getiriliyor: Id = {Id}", id);
            var specialDto = await _specialManager.GetById(id, "Authors");
            if (specialDto == null)
            {
                _logger.LogWarning("Özel içerik bulunamadı: Id = {Id}", id);
                return NotFound($"Special with ID {id} not found.");
            }
            return Ok(specialDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSpecials()
        {
            _logger.LogInformation("Tüm özel içerikler getiriliyor.");
            var specials = await _specialManager.GetAllSpecial("Authors");
            if (specials == null || specials.Count == 0)
            {
                _logger.LogWarning("Hiç özel içerik bulunamadı.");
                return Ok(new List<SpecialGetDto>());
            }
            return Ok(specials);
        }

        [HttpPost]
        public async Task<IActionResult> AddSpecial([FromForm] SpecialPostDto dto)
        {
            _logger.LogInformation("Yeni özel içerik ekleniyor: {@Special}", dto);

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

            await _specialManager.AddSpecial(dto);
            _memoryCache.Remove("SpecialList");

            return StatusCode(StatusCodes.Status201Created, new { message = "Özel içerik başarıyla eklendi!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpecial(int id, [FromForm] SpecialPutDto dto)
        {
            _logger.LogInformation("Özel içerik güncelleniyor: Id = {Id}, Yeni Değerler = {@Dto}", id, dto);

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

            await _specialManager.UpdateSpecial(dto);
            _memoryCache.Remove("SpecialList");
            return Ok(new { message = "Özel içerik başarıyla güncellendi!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecial(int id)
        {
            _logger.LogWarning("Özel içerik siliniyor: Id = {Id}", id);
            await _specialManager.DeleteSpecial(id);
            _memoryCache.Remove("SpecialList");
            return Ok(new { message = "Özel içerik başarıyla silindi!" });
        }
    }
}
