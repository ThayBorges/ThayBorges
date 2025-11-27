using FluentValidation;
using WasteManagement.Application.Contracts.Common;

namespace WasteManagement.Application.Validators;

public class PaginationQueryValidator : AbstractValidator<PaginationQuery>
{
    public PaginationQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page deve ser maior que zero");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("PageSize deve ser maior que zero")
            .LessThanOrEqualTo(100).WithMessage("PageSize máximo é 100");
    }
}
