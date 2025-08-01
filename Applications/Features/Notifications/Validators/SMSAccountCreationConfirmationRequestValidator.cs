using Applications.DTOs;
using FluentValidation;

namespace Applications.Features.Notifications.Validators
{
    public class SMSAccountCreationConfirmationRequestValidator : AbstractValidator<SMSAccountCreationConfirmationRequestDto>
    {
        public SMSAccountCreationConfirmationRequestValidator()
        {
            RuleFor(x => x.ShopName).NotEmpty().WithMessage("Shop name is required");
            RuleFor(x => x.ShopCode).NotEmpty().WithMessage("Shop code is required");
            RuleFor(x => x.ShopOwnerName).NotEmpty().WithMessage("Shop owner name is required");
            RuleFor(x => x.ShopOwnerPhone).Matches(@"^\d{10}$").WithMessage("Shop owner phone must be a valid 10-digit number");
            RuleFor(x => x.ShopAddress).NotEmpty().WithMessage("Shop address is required");
        }
    }
}
