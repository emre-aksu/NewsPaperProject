using FluentValidation;
using ModelLayer.Dtos.AnnouncementDtos;

public class AnnouncementPutDtoValidator : AbstractValidator<AnnouncementPutDto>
{
    public AnnouncementPutDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id geçerli olmalıdır.");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş olamaz.");
        RuleFor(x => x.Institution).NotEmpty().WithMessage("Kurum boş olamaz.");
        RuleFor(x => x.PublishedDate).LessThanOrEqualTo(DateTime.Now).WithMessage("Yayınlanma tarihi gelecekte olamaz.");
        RuleFor(x => x.DeadlineDate).GreaterThan(x => x.PublishedDate).WithMessage("Son başvuru tarihi, yayınlanma tarihinden sonra olmalıdır.");
        RuleFor(x => x.Type).NotEmpty().WithMessage("Tür boş olamaz.");
        RuleFor(x => x.SeoTitle).NotEmpty().WithMessage("SEO başlık boş olamaz.");
        RuleFor(x => x.SeoDescription).NotEmpty().WithMessage("SEO açıklama boş olamaz.");
        RuleFor(x => x.CreatedDate).LessThanOrEqualTo(DateTime.Now).WithMessage("Oluşturulma tarihi gelecekte olamaz.");
        RuleFor(x => x.UpdatedDate).GreaterThanOrEqualTo(x => x.CreatedDate).WithMessage("Güncellenme tarihi, oluşturulma tarihinden önce olamaz.");
        RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Kategori ID geçerli olmalıdır.");
        RuleFor(x => x.AuthorId).GreaterThan(0).WithMessage("Yazar ID geçerli olmalıdır.");
    }
}
