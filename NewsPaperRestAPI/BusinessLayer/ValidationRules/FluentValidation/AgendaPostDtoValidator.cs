using FluentValidation;
using Microsoft.AspNetCore.Http;
using ModelLayer.Dtos.AgendaDtos;
using System;

namespace BusinessLayer.ValidationRules.FluentValidation
{
    public class AgendaPostDtoValidator : AbstractValidator<AgendaPostDto>
    {
        public AgendaPostDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz.")
                .MaximumLength(150).WithMessage("Başlık en fazla 150 karakter olabilir.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama boş olamaz.")
                .MaximumLength(300).WithMessage("Açıklama en fazla 300 karakter olabilir.");

            RuleFor(x => x.Summary)
                .NotEmpty().WithMessage("Özet boş olamaz.")
                .MaximumLength(500).WithMessage("Özet en fazla 500 karakter olabilir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik boş olamaz.");

            RuleFor(x => x.PublishedDate)
                .NotEmpty().WithMessage("Yayın tarihi boş olamaz.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Yayın tarihi bugünden büyük olamaz.");

            RuleFor(x => x.UpdatedDate)
                .NotEmpty().WithMessage("Güncellenme tarihi boş olamaz.")
                .GreaterThanOrEqualTo(x => x.PublishedDate).WithMessage("Güncellenme tarihi, yayın tarihinden önce olamaz.");

            RuleFor(x => x.ViewCount)
                .GreaterThanOrEqualTo(0).WithMessage("Görüntülenme sayısı negatif olamaz.");

            RuleFor(x => x.CreatedDate)
               .NotEmpty().WithMessage("Oluşturulma tarihi boş olamaz.");

            RuleFor(x => x.LikeCount)
                .GreaterThanOrEqualTo(0).WithMessage("Beğeni sayısı negatif olamaz.");

            RuleFor(x => x.CoverImage)
                .NotEmpty().WithMessage("Kapak resmi URL boş olamaz.");

            RuleFor(x => x.Picture)
                .NotNull().WithMessage("Resim yüklenmelidir.")
                .Must(p => p.Length > 0).WithMessage("Resim dosyası boş olamaz.")
                .Must(p => p.ContentType.StartsWith("image/")).WithMessage("Yalnızca resim dosyaları yükleyebilirsiniz.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Geçerli bir kategori seçmelisiniz.");

            RuleFor(x => x.AuthorId)
                .GreaterThan(0).WithMessage("Geçerli bir yazar seçmelisiniz.");
        }
    }
}
