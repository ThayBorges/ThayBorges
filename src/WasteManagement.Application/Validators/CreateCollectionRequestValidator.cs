using FluentValidation;
using WasteManagement.Application.Contracts.CollectionRequests;

namespace WasteManagement.Application.Validators;

public class CreateCollectionRequestValidator : AbstractValidator<CreateCollectionRequest>
{
    public CreateCollectionRequestValidator()
    {
        RuleFor(x => x.CollectionPointId)
            .NotEmpty();

        RuleFor(x => x.EstimatedVolumeKg)
            .GreaterThan(0);

        RuleFor(x => x.Priority)
            .NotEmpty();
    }
}
