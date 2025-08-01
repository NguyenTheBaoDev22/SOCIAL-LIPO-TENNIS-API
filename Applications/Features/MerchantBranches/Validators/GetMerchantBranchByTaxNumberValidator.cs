using Applications.Commons.Validators;
using Applications.Features.MerchantBranches.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.MerchantBranches.Validators
{
    public class GetMerchantBranchByTaxNumberValidator : BaseValidator<GetMerchantBranchByTaxNumberQuery>
    {
        public GetMerchantBranchByTaxNumberValidator()
        {
            RuleFor(x => x.TaxNumber)
            .NotEmpty()
            .Matches(@"^\d{10}$|^\d{13}$|^\d{10}-\d{3}$")
            .WithMessage("Mã số thuế phải là 10 số, 13 số liền nhau, hoặc 10 số + '-' + 3 số (dấu '-' phải nằm ở vị trí thứ 11).");
        }
    }
}
