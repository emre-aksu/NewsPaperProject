using FluentValidation;
using ModelLayer.Dtos.HomeDtos;

public class HomePutDtoValidator : AbstractValidator<HomePutDto>
{
    public HomePutDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id geçerli olmalıdır.");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş olamaz.");
        RuleFor(x => x.Summary).NotEmpty().WithMessage("Özet boş olamaz.");
        RuleFor(x => x.Content).NotEmpty().WithMessage("İçerik boş olamaz.");
        RuleFor(x => x.PublishedDate).NotEmpty().WithMessage("Yayınlanma tarihi boş olamaz.");
        RuleFor(x => x.UpdatedDate).NotEmpty().WithMessage("Güncellenme tarihi boş olamaz.");
        RuleFor(x => x.Tags).NotEmpty().WithMessage("Etiketler boş olamaz.");
        RuleFor(x => x.ViewCount).GreaterThanOrEqualTo(0).WithMessage("Görüntülenme sayısı negatif olamaz.");
        RuleFor(x => x.CommentCount).GreaterThanOrEqualTo(0).WithMessage("Yorum sayısı negatif olamaz.");
        RuleFor(x => x.LikeCount).GreaterThanOrEqualTo(0).WithMessage("Beğeni sayısı negatif olamaz.");
        RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Kategori Id geçerli olmalıdır.");
        RuleFor(x => x.AuthorId).GreaterThan(0).WithMessage("Yazar Id geçerli olmalıdır.");
    }
}
