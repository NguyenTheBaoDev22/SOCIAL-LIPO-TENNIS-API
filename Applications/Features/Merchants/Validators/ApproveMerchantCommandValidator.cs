using Applications.Features.Merchants.Commands.Applications.Features.Merchants.Commands;
using FluentValidation;

namespace Applications.Features.Merchants.Validators
{
    public class ApproveMerchantCommandValidator : AbstractValidator<ApproveMerchantCommand>
    {
        public ApproveMerchantCommandValidator()
        {
            RuleFor(x => x.MerchantBranchId).NotEmpty();
        }
    }
}
