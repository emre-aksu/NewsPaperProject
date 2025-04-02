using FluentValidation;
using Microsoft.AspNetCore.Http;
using ModelLayer.Dtos.HomeDtos;

namespace BusinessLayer.ValidationRules.FluentValidation
{
    public class HomePostDtoValidator : AbstractValidator<HomePostDto>
    {
        public HomePostDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz.")
                .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.");

            RuleFor(x => x.Summary)
                .NotEmpty().WithMessage("Özet boş olamaz.")
                .MaximumLength(500).WithMessage("Özet en fazla 500 karakter olabilir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik boş olamaz.")
                .MinimumLength(20).WithMessage("İçerik en az 20 karakter olmalıdır.");

            RuleFor(x => x.PublishedDate)
                .NotEmpty().WithMessage("Yayınlanma tarihi boş olamaz.");

            RuleFor(x => x.UpdatedDate)
                .NotEmpty().WithMessage("Güncellenme tarihi boş olamaz.")
                .GreaterThanOrEqualTo(x => x.PublishedDate).WithMessage("Güncellenme tarihi, yayınlanma tarihinden önce olamaz.");

            RuleFor(x => x.Tags)
                .NotEmpty().WithMessage("Etiketler boş olamaz.")
                .MaximumLength(300).WithMessage("Etiketler en fazla 300 karakter olabilir.");

            RuleFor(x => x.ViewCount)
                .GreaterThanOrEqualTo(0).WithMessage("Görüntülenme sayısı negatif olamaz.");

            RuleFor(x => x.CommentCount)
                .GreaterThanOrEqualTo(0).WithMessage("Yorum sayısı negatif olamaz.");

            RuleFor(x => x.LikeCount)
                .GreaterThanOrEqualTo(0).WithMessage("Beğeni sayısı negatif olamaz.");

            RuleFor(x => x.CoverImage)
                .NotEmpty().WithMessage("Kapak resmi boş olamaz.");

            RuleFor(x => x.Picture)
                .NotNull().WithMessage("Resim yüklenmelidir.")
                .Must(p => p.Length > 0).WithMessage("Resim dosyası boş olamaz.")
                .Must(p => p.ContentType.StartsWith("image/")).WithMessage("Yalnızca resim dosyaları yükleyebilirsiniz.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Geçerli bir kategori seçilmelidir.");

            RuleFor(x => x.AuthorId)
                .GreaterThan(0).WithMessage("Geçerli bir yazar seçilmelidir.");
        }
    }
}
