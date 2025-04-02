using FluentValidation;
using ModelLayer.Dtos.CategoryDtos;

namespace BusinessLayer.ValidationRules.FluentValidation
{
    public class CategoryPostDtoValidator : AbstractValidator<CategoryPostDto>
    {
        public CategoryPostDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori adı boş olamaz.")
                .MaximumLength(100).WithMessage("Kategori adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama boş olamaz.")
                .MinimumLength(10).WithMessage("Açıklama en az 10 karakter olmalıdır.");

            RuleFor(x => x.Picture)
                .NotNull().WithMessage("Resim yüklenmelidir.")
                .Must(p => p.Length > 0).WithMessage("Resim dosyası boş olamaz.")
                .Must(p => p.ContentType.StartsWith("image/")).WithMessage("Yalnızca resim dosyaları yükleyebilirsiniz.");

            RuleFor(x => x.PhotoPath)
                .NotEmpty().WithMessage("Fotoğraf yolu boş olamaz.");
        }
    }
}
