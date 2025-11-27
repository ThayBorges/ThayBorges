using FluentValidation;
using WasteManagement.Application.Contracts.CollectionPoints;

namespace WasteManagement.Application.Validators;

public class UpdateCollectionPointRequestValidator : AbstractValidator<UpdateCollectionPointRequest>
{
    public UpdateCollectionPointRequestValidator()
    {
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
