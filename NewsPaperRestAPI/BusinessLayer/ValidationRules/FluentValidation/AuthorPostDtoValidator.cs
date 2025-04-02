using FluentValidation;
using ModelLayer.Dtos.AuthorDtos;

namespace BusinessLayer.ValidationRules.FluentValidation
{
    public class AuthorPostDtoValidator : AbstractValidator<AuthorPostDto>
    {
        public AuthorPostDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad boş olamaz.")
                .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad boş olamaz.")
                .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir.");

            RuleFor(x => x.Biography)
                .NotEmpty().WithMessage("Biyografi boş olamaz.")
                .MinimumLength(10).WithMessage("Biyografi en az 50 karakter olmalıdır.");

            RuleFor(x => x.CoverImage)
                .NotEmpty().WithMessage("Kapak resmi boş olamaz.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon numarası boş olamaz.")
                .Matches(@"^\+?\d{10,15}$").WithMessage("Geçerli bir telefon numarası giriniz.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta adresi boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.Picture)
                .NotNull().WithMessage("Resim yüklenmelidir.")
                .Must(p => p.Length > 0).WithMessage("Resim dosyası boş olamaz.")
                .Must(p => p.ContentType.StartsWith("image/")).WithMessage("Yalnızca resim dosyaları yükleyebilirsiniz.");
        }
    }
}

