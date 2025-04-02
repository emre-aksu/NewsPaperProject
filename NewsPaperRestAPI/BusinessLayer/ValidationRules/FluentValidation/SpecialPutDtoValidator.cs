using FluentValidation;
using ModelLayer.Dtos.SpecialDtos;

public class SpecialPutDtoValidator : AbstractValidator<SpecialPutDto>
{
    public SpecialPutDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id geçerli olmalıdır.");
        RuleFor(x => x.PageName).NotEmpty().WithMessage("Sayfa adı boş olamaz.");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş olamaz.");
        RuleFor(x => x.Content).NotEmpty().WithMessage("İçerik boş olamaz.");
        RuleFor(x => x.SeoTitle).NotEmpty().WithMessage("Seo başlık boş olamaz.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama boş olamaz.");
        RuleFor(x => x.SeoKeywords).NotEmpty().WithMessage("Seo anahtar kelimeler boş olamaz.");
        RuleFor(x => x.CreatedDate).NotEmpty().WithMessage("Oluşturulma tarihi boş olamaz.");
        RuleFor(x => x.UpdateDate).NotEmpty().WithMessage("Güncellenme tarihi boş olamaz.");
        RuleFor(x => x.AuthorId).GreaterThan(0).WithMessage("Yazar Id geçerli olmalıdır.");
        RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Kategori Id geçerli olmalıdır.");
    }
}
