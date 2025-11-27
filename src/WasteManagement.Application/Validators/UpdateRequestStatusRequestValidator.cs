using FluentValidation;
using WasteManagement.Application.Contracts.CollectionRequests;
using WasteManagement.Domain.Enums;

namespace WasteManagement.Application.Validators;

public class UpdateRequestStatusRequestValidator : AbstractValidator<UpdateRequestStatusRequest>
{
    public UpdateRequestStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum();

        When(x => x.Status == RequestStatus.Completed, () =>
        {
            RuleFor(x => x.CompletedAtUtc)
                .NotNull();
        });
    }
}
