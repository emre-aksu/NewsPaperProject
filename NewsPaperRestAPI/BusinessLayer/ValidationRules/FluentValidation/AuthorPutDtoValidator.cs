using FluentValidation;
using ModelLayer.Dtos.AuthorDtos;

public class AuthorPutDtoValidator : AbstractValidator<AuthorPutDto>
{
    public AuthorPutDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id geçerli olmalıdır.");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad boş olamaz.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad boş olamaz.");
        RuleFor(x => x.Biography).NotEmpty().WithMessage("Biyografi boş olamaz.");
        RuleFor(x => x.Phone).Matches(@"^\+?\d{10,15}$").WithMessage("Telefon numarası geçerli olmalıdır.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");
    }
}
