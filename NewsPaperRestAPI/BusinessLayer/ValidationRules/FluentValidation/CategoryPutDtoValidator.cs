using FluentValidation;
using ModelLayer.Dtos.CategoryDtos;

public class CategoryPutDtoValidator : AbstractValidator<CategoryPutDto>
{
    public CategoryPutDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id geçerli olmalıdır.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Kategori adı boş olamaz.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama boş olamaz.");
    }
}
