using FluentValidation;
using ModelLayer.Dtos.SportDtos;

namespace BusinessLayer.ValidationRules.FluentValidation
{
    public class SportPostDtoValidator : AbstractValidator<SportPostDto>
    {
        public SportPostDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz.")
                .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik boş olamaz.")
                .MinimumLength(20).WithMessage("İçerik en az 20 karakter olmalıdır.");

            RuleFor(x => x.PublicationDate)
                .NotEmpty().WithMessage("Yayınlanma tarihi boş olamaz.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Tür boş olamaz.")
                .MaximumLength(100).WithMessage("Tür en fazla 100 karakter olabilir.");

            RuleFor(x => x.CoverImage)
                .NotEmpty().WithMessage("Kapak resmi boş olamaz.");

            RuleFor(x => x.Picture)
                .NotNull().WithMessage("Resim yüklenmelidir.")
                .Must(p => p.Length > 0).WithMessage("Resim dosyası boş olamaz.")
                .Must(p => p.ContentType.StartsWith("image/")).WithMessage("Yalnızca resim dosyaları yükleyebilirsiniz.");

            RuleFor(x => x.SeoTitle)
                .NotEmpty().WithMessage("SEO başlık boş olamaz.")
                .MaximumLength(100).WithMessage("SEO başlık en fazla 100 karakter olabilir.");

            RuleFor(x => x.Status)
                .NotNull().WithMessage("Durum boş olamaz.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Geçerli bir kategori seçilmelidir.");

            RuleFor(x => x.AuthorId)
                .GreaterThan(0).WithMessage("Geçerli bir yazar seçilmelidir.");
        }
    }
}
