using BusinessLayer.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ModelLayer.Dtos.HomeDtos;

namespace PresentationsWebAPI.Controllers
{
    [Route("api/homes")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeManager _homeManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IHomeManager homeManager, ILogger<HomeController> logger, IMemoryCache memoryCache, IWebHostEnvironment webHostEnvironment)
        {
            _homeManager = homeManager;
            _logger = logger;
            _memoryCache = memoryCache;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Ev getiriliyor: Id = {Id}", id);
            var homeDto = await _homeManager.GetById(id, "Authors");
            if (homeDto == null)
            {
                _logger.LogWarning("Ev bulunamadı: Id = {Id}", id);
                return NotFound($"Home with ID {id} not found.");
            }
            return Ok(homeDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHomes()
        {
            _logger.LogInformation("Tüm evler getiriliyor.");
            var homes = await _homeManager.GetAllHome("Authors");
            if (homes == null || homes.Count == 0)
            {
                _logger.LogWarning("Hiç ev bulunamadı.");
                return Ok(new List<HomeGetDto>());
            }
            return Ok(homes);
        }

        [HttpPost]
        public async Task<IActionResult> AddHome([FromForm] HomePostDto dto)
        {
            _logger.LogInformation("Yeni bir ev ekleniyor: {@Home}", dto);

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

            await _homeManager.AddHome(dto);
            _memoryCache.Remove("HomeList");

            return StatusCode(StatusCodes.Status201Created, new { message = "Ev başarıyla eklendi!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHome(int id, [FromForm] HomePutDto dto)
        {
            _logger.LogInformation("Ev güncelleniyor: Id = {Id}, Yeni Değerler = {@Dto}", id, dto);

            var existingHome = await _homeManager.GetById(id);
            if (existingHome == null)
            {
                _logger.LogWarning("Güncellenmek istenen ev bulunamadı: Id = {Id}", id);
                return NotFound($"Home with ID {id} not found.");
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

                    var fileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.Picture.CopyToAsync(stream);
                    }

                    dto.CoverImage = $"/uploads/{fileName}"; // Ev için kapak resmi alanı
                }
                catch (Exception ex)
                {
                    _logger.LogError("Dosya yükleme hatası: {Message}", ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dosya yüklenirken bir hata oluştu.");
                }
            }
            else
            {
                dto.CoverImage = existingHome.CoverImage; // Yeni resim yüklenmezse eskiyi koru
            }

            await _homeManager.UpdateHome(dto);
            _memoryCache.Remove("HomeList");

            return Ok(new { message = "Ev başarıyla güncellendi!" });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHome(int id)
        {
            _logger.LogWarning("Ev siliniyor: Id = {Id}", id);
            await _homeManager.DeleteHome(id);
            _memoryCache.Remove("HomeList");
            return Ok(new { message = "Ev başarıyla silindi!" });
        }
    }
}
