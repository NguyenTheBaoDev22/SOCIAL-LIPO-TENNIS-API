namespace Applications.Features.Merchants.Validators
{
    using Core.Enums;
    using FluentValidation;

    public class RegisterMerchantWithBranchCommandValidator : AbstractValidator<RegisterMerchantWithBranchCommand>
    {
        public RegisterMerchantWithBranchCommandValidator()
        {
            //RuleFor(x => x.MerchantCategoryCode)
            //    .NotEmpty().WithMessage("Merchant Category Code is required.")
            //    .Must(BeAValidCategoryCode)
            //    .WithMessage("Invalid Merchant Category Code.");
        }

        private bool BeAValidCategoryCode(string categoryCode)
        {
            return MerchantCategoryCodes.All.ContainsKey(categoryCode);
        }
    }


}
