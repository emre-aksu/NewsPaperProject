using FluentValidation;
using ModelLayer.Dtos.AgendaDtos;

public class AgendaPutDtoValidator : AbstractValidator<AgendaPutDto>
{
    public AgendaPutDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Summary).MaximumLength(500);
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.PublishedDate).NotEmpty();
        RuleFor(x => x.UpdatedDate).NotEmpty();
        RuleFor(x => x.ViewCount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.LikeCount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CoverImage).NotEmpty();
        RuleFor(x => x.Status).NotNull();
        RuleFor(x => x.CategoryId).GreaterThan(0);
        RuleFor(x => x.AuthorId).GreaterThan(0);
    }
}
