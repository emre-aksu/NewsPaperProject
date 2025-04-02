using FluentValidation;
using ModelLayer.Dtos.AnnouncementDtos;

namespace BusinessLayer.ValidationRules.FluentValidation
{
    public class AnnouncementPostDtoValidator : AbstractValidator<AnnouncementPostDto>
    {
        public AnnouncementPostDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz.")
                .MaximumLength(150).WithMessage("Başlık en fazla 150 karakter olabilir.");

            RuleFor(x => x.Institution)
                .NotEmpty().WithMessage("Kurum adı boş olamaz.")
                .MaximumLength(100).WithMessage("Kurum adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.PublicationDate)
                .NotEmpty().WithMessage("Yayın tarihi boş olamaz.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Yayın tarihi bugünden büyük olamaz.");

            RuleFor(x => x.DeadlineDate)
                .NotEmpty().WithMessage("Son başvuru tarihi boş olamaz.")
                .GreaterThan(x => x.PublicationDate).WithMessage("Son başvuru tarihi yayın tarihinden sonra olmalıdır.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Tür boş olamaz.")
                .MaximumLength(50).WithMessage("Tür en fazla 50 karakter olabilir.");

            RuleFor(x => x.CoverImage)
                .NotEmpty().WithMessage("Kapak resmi URL boş olamaz.");

            RuleFor(x => x.SeoTitle)
                .NotEmpty().WithMessage("SEO başlık boş olamaz.")
                .MaximumLength(150).WithMessage("SEO başlık en fazla 150 karakter olabilir.");

            RuleFor(x => x.SeoDescription)
                .NotEmpty().WithMessage("SEO açıklama boş olamaz.")
                .MaximumLength(300).WithMessage("SEO açıklama en fazla 300 karakter olabilir.");

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
