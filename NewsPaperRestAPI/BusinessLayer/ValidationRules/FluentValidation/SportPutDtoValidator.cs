using FluentValidation;
using ModelLayer.Dtos.SportDtos;

public class SportPutDtoValidator : AbstractValidator<SportPutDto>
{
    public SportPutDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id geçerli olmalıdır.");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş olamaz.");
        RuleFor(x => x.Content).NotEmpty().WithMessage("İçerik boş olamaz.");
        RuleFor(x => x.PublicationDate).NotEmpty().WithMessage("Yayınlanma tarihi boş olamaz.");
        RuleFor(x => x.Type).NotEmpty().WithMessage("Tür boş olamaz.");
        RuleFor(x => x.SeoTitle).NotEmpty().WithMessage("Seo başlık boş olamaz.");
        RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Kategori Id geçerli olmalıdır.");
        RuleFor(x => x.AuthorId).GreaterThan(0).WithMessage("Yazar Id geçerli olmalıdır.");
    }
}
