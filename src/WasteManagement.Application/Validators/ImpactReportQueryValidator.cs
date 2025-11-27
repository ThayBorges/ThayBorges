using FluentValidation;
using WasteManagement.Application.Contracts.Impact;

namespace WasteManagement.Application.Validators;

public class ImpactReportQueryValidator : AbstractValidator<ImpactReportQuery>
{
    public ImpactReportQueryValidator()
    {
        RuleFor(x => x.End)
            .Must((model, end) => !end.HasValue || !model.Start.HasValue || model.Start <= end)
            .WithMessage("Data final deve ser maior ou igual à data inicial");
    }
}
