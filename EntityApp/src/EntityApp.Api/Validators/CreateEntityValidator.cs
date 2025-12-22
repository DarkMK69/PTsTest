using FluentValidation;
using EntityApp.Api.Dtos;

namespace EntityApp.Api.Validators;

public class CreateEntityValidator : AbstractValidator<CreateEntityDto>
{
    public CreateEntityValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Tags)
            .Must(tags => tags == null || tags.All(t => !string.IsNullOrWhiteSpace(t)))
            .When(x => x.Tags != null);
    }
}