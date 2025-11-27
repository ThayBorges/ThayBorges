using FluentValidation;
using WasteManagement.Application.Contracts.Alerts;

namespace WasteManagement.Application.Validators;

public class ResolveAlertRequestValidator : AbstractValidator<ResolveAlertRequest>
{
    public ResolveAlertRequestValidator()
    {
        RuleFor(x => x.Resolved)
            .NotNull();
    }
}
