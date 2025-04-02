using BusinessLayer.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ModelLayer.Dtos.AnnouncementDtos;

namespace PresentationsWebAPI.Controllers
{
    [Route("api/announcements")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementManager _announcementManager;
        private readonly ILogger<AnnouncementController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AnnouncementController(IAnnouncementManager announcementManager, ILogger<AnnouncementController> logger, IMemoryCache memoryCache, IWebHostEnvironment webHostEnvironment)
        {
            _announcementManager = announcementManager;
            _logger = logger;
            _memoryCache = memoryCache;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Duyuru getiriliyor: Id = {Id}", id);
            var announcementDto = await _announcementManager.GetById(id, "Authors");
            if (announcementDto == null)
            {
                _logger.LogWarning("Duyuru bulunamadı: Id = {Id}", id);
                return NotFound($"Duyuru with ID {id} not found.");
            }
            return Ok(announcementDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnnouncements()
        {
            _logger.LogInformation("Tüm duyurular getiriliyor.");
            var announcements = await _announcementManager.GetAllAnnouncement("Authors");
            if (announcements == null || announcements.Count == 0)
            {
                _logger.LogWarning("Hiç duyuru bulunamadı.");
                return Ok(new List<AnnouncementGetDto>());
            }
            return Ok(announcements);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnnouncement([FromForm] AnnouncementPostDto dto)
        {
            _logger.LogInformation("Yeni bir duyuru ekleniyor: {@Announcement}", dto);

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

            await _announcementManager.AddAnnouncement(dto);
            _memoryCache.Remove("AnnouncementList");

            return StatusCode(StatusCodes.Status201Created, new { message = "Duyuru başarıyla eklendi!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnnouncement(int id, [FromForm] AnnouncementPutDto dto)
        {
            _logger.LogInformation("Duyuru güncelleniyor: Id = {Id}, Yeni Değerler = {@Dto}", id, dto);

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

            await _announcementManager.UpdateAnnouncement(dto);
            _memoryCache.Remove("AnnouncementList");
            return Ok(new { message = "Duyuru başarıyla güncellendi!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            _logger.LogWarning("Duyuru siliniyor: Id = {Id}", id);
            await _announcementManager.DeleteAnnouncement(id);
            _memoryCache.Remove("AnnouncementList");
            return Ok(new { message = "Duyuru başarıyla silindi!" });
        }
    }
}
