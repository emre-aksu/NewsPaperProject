using FluentValidation;
using Microsoft.AspNetCore.Http;
using ModelLayer.Dtos.SpecialDtos;

namespace BusinessLayer.ValidationRules.FluentValidation
{
    public class SpecialPostDtoValidator : AbstractValidator<SpecialPostDto>
    {
        public SpecialPostDtoValidator()
        {
            RuleFor(x => x.PageName)
                .NotEmpty().WithMessage("Sayfa adı boş olamaz.")
                .MaximumLength(150).WithMessage("Sayfa adı en fazla 150 karakter olabilir.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz.")
                .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik boş olamaz.")
                .MinimumLength(20).WithMessage("İçerik en az 20 karakter olmalıdır.");

            RuleFor(x => x.CoverImage)
                .NotEmpty().WithMessage("Kapak resmi boş olamaz.");

            RuleFor(x => x.Picture)
                .NotNull().WithMessage("Resim yüklenmelidir.")
                .Must(p => p.Length > 0).WithMessage("Resim dosyası boş olamaz.")
                .Must(p => p.ContentType.StartsWith("image/")).WithMessage("Yalnızca resim dosyaları yükleyebilirsiniz.");

            RuleFor(x => x.SeoTitle)
                .NotEmpty().WithMessage("SEO başlık boş olamaz.")
                .MaximumLength(100).WithMessage("SEO başlık en fazla 100 karakter olabilir.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama boş olamaz.")
                .MaximumLength(300).WithMessage("SEO açıklama en fazla 300 karakter olabilir.");

            RuleFor(x => x.SeoKeywords)
                .NotEmpty().WithMessage("SEO anahtar kelimeler boş olamaz.")
                .MaximumLength(300).WithMessage("SEO anahtar kelimeler en fazla 300 karakter olabilir.");

            RuleFor(x => x.CreatedDate)
                .NotEmpty().WithMessage("Oluşturulma tarihi boş olamaz.");

            RuleFor(x => x.UpdateDate)
                .NotEmpty().WithMessage("Güncellenme tarihi boş olamaz.")
                .GreaterThanOrEqualTo(x => x.CreatedDate).WithMessage("Güncellenme tarihi, oluşturulma tarihinden önce olamaz.");

            RuleFor(x => x.AuthorId)
                .GreaterThan(0).WithMessage("Geçerli bir yazar seçilmelidir.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Geçerli bir kategori seçilmelidir.");
        }
    }
}
