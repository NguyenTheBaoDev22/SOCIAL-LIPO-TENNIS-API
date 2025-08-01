using Core.Enums;
using FluentValidation;

namespace Applications.Features.Merchants.Validators
{
    public class AddMerchantBranchCommandValidator : AbstractValidator<AddMerchantBranchCommand>
    {
        public AddMerchantBranchCommandValidator()
        {
            RuleFor(x => x.MerchantCategoryCode)
                .NotEmpty().WithMessage("Merchant Category Code is required.")
                .Must(BeAValidCategoryCode)
                .WithMessage("Invalid Merchant Category Code.");
        }

        private bool BeAValidCategoryCode(string categoryCode)
        {
            return MerchantCategoryCodes.All.ContainsKey(categoryCode);
        }
    }
}
