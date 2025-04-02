using BusinessLayer.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ModelLayer.Dtos.AuthorDtos;

namespace PresentationsWebAPI.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorManager _authorManager;
        private readonly ILogger<AuthorController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthorController(IAuthorManager authorManager, ILogger<AuthorController> logger, IMemoryCache memoryCache, IWebHostEnvironment webHostEnvironment)
        {
            _authorManager = authorManager;
            _logger = logger;
            _memoryCache = memoryCache;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Yazar getiriliyor: Id = {Id}", id);
            var authorDto = await _authorManager.GetById(id, "Books");
            if (authorDto == null)
            {
                _logger.LogWarning("Yazar bulunamadı: Id = {Id}", id);
                return NotFound($"Author with ID {id} not found.");
            }
            return Ok(authorDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            _logger.LogInformation("Tüm yazarlar getiriliyor.");
            var authors = await _authorManager.GetAllAuthor("Books");
            if (authors == null || authors.Count == 0)
            {
                _logger.LogWarning("Hiç yazar bulunamadı.");
                return Ok(new List<AuthorGetDto>());
            }
            return Ok(authors);
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromForm] AuthorPostDto dto)
        {
            _logger.LogInformation("Yeni bir yazar ekleniyor: {@Author}", dto);

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

            await _authorManager.AddAuthor(dto);
            _memoryCache.Remove("AuthorList");

            return StatusCode(StatusCodes.Status201Created, new { message = "Yazar başarıyla eklendi!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromForm] AuthorPutDto dto)
        {
            _logger.LogInformation("Yazar güncelleniyor: Id = {Id}, Yeni Değerler = {@Dto}", id, dto);

            var existingAuthor = await _authorManager.GetById(id);
            if (existingAuthor == null)
            {
                _logger.LogWarning("Güncellenmek istenen yazar bulunamadı: Id = {Id}", id);
                return NotFound($"Author with ID {id} not found.");
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

                    dto.CoverImage = $"/uploads/{fileName}"; // Author için profil resmi alanı
                }
                catch (Exception ex)
                {
                    _logger.LogError("Dosya yükleme hatası: {Message}", ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dosya yüklenirken bir hata oluştu.");
                }
            }
            else
            {
                dto.CoverImage = existingAuthor.CoverImage; // Resim yoksa eskiyi koru
            }

            await _authorManager.UpdateAuthor(dto);
            _memoryCache.Remove("AuthorList");

            return Ok(new { message = "Yazar başarıyla güncellendi!" });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            _logger.LogWarning("Yazar siliniyor: Id = {Id}", id);
            await _authorManager.DeleteAuthor(id);
            _memoryCache.Remove("AuthorList");
            return Ok(new { message = "Yazar başarıyla silindi!" });
        }
    }
}
