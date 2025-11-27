using FluentValidation;
using WasteManagement.Application.Contracts.Alerts;

namespace WasteManagement.Application.Validators;

public class CreateAlertRequestValidator : AbstractValidator<CreateAlertRequest>
{
    public CreateAlertRequestValidator()
    {
        RuleFor(x => x.CollectionPointId)
            .NotEmpty();

        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(240);

        RuleFor(x => x.Severity)
            .IsInEnum();
    }
}
