using FluentValidation;
using ModelLayer.Dtos.EconomyDtos;

public class EconomyPutDtoValidator : AbstractValidator<EconomyPutDto>
{
    public EconomyPutDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id geçerli olmalıdır.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Ad boş olamaz.");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş olamaz.");
        RuleFor(x => x.Content).NotEmpty().WithMessage("İçerik boş olamaz.");
        RuleFor(x => x.PublishedDate).NotEmpty().WithMessage("Yayınlanma tarihi boş olamaz.");
        RuleFor(x => x.ExchangeRates).GreaterThan(0).WithMessage("Döviz kurları 0'dan büyük olmalıdır.");
        RuleFor(x => x.GoldPrices).GreaterThan(0).WithMessage("Altın fiyatı 0'dan büyük olmalıdır.");
        RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Kategori Id geçerli olmalıdır.");
        RuleFor(x => x.AuthorId).GreaterThan(0).WithMessage("Yazar Id geçerli olmalıdır.");
    }
}
