using BusinessLayer.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ModelLayer.Dtos.CategoryDtos;

namespace PresentationsWebAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;
        private readonly ILogger<CategoryController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(ICategoryManager categoryManager, ILogger<CategoryController> logger, IMemoryCache memoryCache, IWebHostEnvironment webHostEnvironment)
        {
            _categoryManager = categoryManager;
            _logger = logger;
            _memoryCache = memoryCache;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Kategori getiriliyor: Id = {Id}", id);
            var categoryDto = await _categoryManager.GetById(id);
            if (categoryDto == null)
            {
                _logger.LogWarning("Kategori bulunamadı: Id = {Id}", id);
                return NotFound($"Category with ID {id} not found.");
            }
            return Ok(categoryDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            _logger.LogInformation("Tüm kategoriler getiriliyor.");
            var categories = await _categoryManager.GetAllCategories();
            if (categories == null || categories.Count == 0)
            {
                _logger.LogWarning("Hiç kategori bulunamadı.");
                return Ok(new List<CategoryGetDto>());
            }
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromForm] CategoryPostDto dto)
        {
            _logger.LogInformation("Yeni bir kategori ekleniyor: {@Category}", dto);

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

                dto.PhotoPath = $"/uploads/{fileName}";
            }

            await _categoryManager.AddCategory(dto);
            _memoryCache.Remove("CategoryList");

            return StatusCode(StatusCodes.Status201Created, new { message = "Kategori başarıyla eklendi!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryPutDto dto)
        {
            _logger.LogInformation("Kategori güncelleniyor: Id = {Id}, Yeni Değerler = {@Dto}", id, dto);

            var existingCategory = await _categoryManager.GetById(id);
            if (existingCategory == null)
            {
                _logger.LogWarning("Güncellenmek istenen kategori bulunamadı: Id = {Id}", id);
                return NotFound($"Category with ID {id} not found.");
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

                    dto.PhotoPath = $"/uploads/{fileName}";
                }
                catch (Exception ex)
                {
                    _logger.LogError("Dosya yükleme hatası: {Message}", ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dosya yüklenirken bir hata oluştu.");
                }
            }
            else
            {
                dto.PhotoPath = existingCategory.PhotoPath; // Resim yoksa eskiyi koru
            }

            await _categoryManager.UpdateCategory(dto);
            _memoryCache.Remove("CategoryList");

            return Ok(new { message = "Kategori başarıyla güncellendi!" });
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            _logger.LogWarning("Kategori siliniyor: Id = {Id}", id);
            await _categoryManager.DeleteCategory(id);
            _memoryCache.Remove("CategoryList");
            return Ok(new { message = "Kategori başarıyla silindi!" });
        }
    }
}
