using FluentValidation;
using WasteManagement.Application.Contracts.CollectionPoints;

namespace WasteManagement.Application.Validators;

public class CreateCollectionPointRequestValidator : AbstractValidator<CreateCollectionPointRequest>
{
    public CreateCollectionPointRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Neighborhood)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.CapacityKg)
            .GreaterThan(0);

        RuleFor(x => x.CurrentLoadKg)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(x => x.CapacityKg);

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180);
    }
}
