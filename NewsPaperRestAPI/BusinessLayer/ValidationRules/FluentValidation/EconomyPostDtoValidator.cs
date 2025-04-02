using FluentValidation;
using Microsoft.AspNetCore.Http;
using ModelLayer.Dtos.EconomyDtos;

namespace BusinessLayer.ValidationRules.FluentValidation
{
    public class EconomyPostDtoValidator : AbstractValidator<EconomyPostDto>
    {
        public EconomyPostDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("İsim boş olamaz.")
                .MaximumLength(100).WithMessage("İsim en fazla 100 karakter olabilir.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama boş olamaz.")
                .MinimumLength(10).WithMessage("Açıklama en az 10 karakter olmalıdır.");

            RuleFor(x => x.Picture)
                .NotNull().WithMessage("Resim yüklenmelidir.")
                .Must(p => p.Length > 0).WithMessage("Resim dosyası boş olamaz.")
                .Must(p => p.ContentType.StartsWith("image/")).WithMessage("Yalnızca resim dosyaları yükleyebilirsiniz.");

            RuleFor(x => x.CoverImage)
                .NotEmpty().WithMessage("Kapak resmi boş olamaz.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik boş olamaz.")
                .MinimumLength(20).WithMessage("İçerik en az 20 karakter olmalıdır.");

            RuleFor(x => x.PublishedDate)
                .NotEmpty().WithMessage("Yayınlanma tarihi boş olamaz.");

            RuleFor(x => x.UpdateDate)
                .NotEmpty().WithMessage("Güncellenme tarihi boş olamaz.")
                .GreaterThanOrEqualTo(x => x.PublishedDate).WithMessage("Güncellenme tarihi, yayınlanma tarihinden önce olamaz.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz.")
                .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.");

            RuleFor(x => x.ExchangeRates)
                .GreaterThanOrEqualTo(0).WithMessage("Döviz kurları negatif olamaz.");

            RuleFor(x => x.StockIndex)
                .NotEmpty().WithMessage("Hisse senetleri boş olamaz.");

            RuleFor(x => x.GoldPrices)
                .GreaterThanOrEqualTo(0).WithMessage("Altın fiyatları negatif olamaz.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Geçerli bir kategori seçilmelidir.");

            RuleFor(x => x.AuthorId)
                .GreaterThan(0).WithMessage("Geçerli bir yazar seçilmelidir.");
        }
    }
}
