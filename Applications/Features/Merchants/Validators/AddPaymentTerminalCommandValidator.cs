using Applications.Features.Merchants.Commands;
using Core.Enumerables;
using FluentValidation;

namespace Applications.Features.Merchants.Validators
{
    public class AddPaymentTerminalCommandValidator : AbstractValidator<AddPaymentTerminalCommand>
    {
        public AddPaymentTerminalCommandValidator()
        {
            RuleFor(x => x.MerchantId)
                .NotEmpty().WithMessage("MerchantId is required.");

            RuleFor(x => x.MerchantCode)
                .NotEmpty().WithMessage("MerchantCode is required.");

            RuleFor(x => x.MerchantBranchId)
                .NotEmpty().WithMessage("MerchantBranchId is required.");

            RuleFor(x => x.MerchantBranchCode)
                .NotEmpty().WithMessage("MerchantBranchCode is required.");

            RuleFor(x => x.TerminalName)
                .NotEmpty().WithMessage("TerminalName is required.");

            RuleFor(x => x.DeviceType)
                .Must(DeviceTypeConstants.IsValid)
                .WithMessage("Invalid DeviceType. Allowed values: " + string.Join(", ", DeviceTypeConstants.All));
        }
    }



}
