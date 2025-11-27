using FluentValidation;
using WasteManagement.Application.Contracts.CollectionPoints;

namespace WasteManagement.Application.Validators;

public class CollectionPointFilterValidator : AbstractValidator<CollectionPointFilter>
{
    public CollectionPointFilterValidator()
    {
        RuleFor(x => x.MinFillPercentage)
            .InclusiveBetween(0, 100)
            .When(x => x.MinFillPercentage.HasValue);
    }
}
