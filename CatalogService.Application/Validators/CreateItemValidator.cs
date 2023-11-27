using CatalogService.Domain.Entities;
using FluentValidation;

public class CreateItemValidator : AbstractValidator<Item>
{
    public CreateItemValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

        RuleFor(dto => dto.Description)
            .MaximumLength(4000).WithMessage("Description cannot exceed 4000 characters.");

        RuleFor(dto => dto.ImageUrl)
            .Must(BeAValidUrl).When(dto => !string.IsNullOrEmpty(dto.ImageUrl))
            .WithMessage("Image must be a valid URL.");

        RuleFor(dto => dto.CategoryId)
            .NotEmpty().WithMessage("Category is required.");

        RuleFor(dto => dto.Price)
            .NotEmpty().WithMessage("Price is required.")
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(dto => dto.Amount)
            .NotEmpty().WithMessage("Amount is required.")
            .GreaterThan(0).WithMessage("Amount must be a positive integer.");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}
