using Applications.Features.Merchants.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Applications.Features.Merchants.Validators
{
    public class LookupTaxCodeQueryValidator : AbstractValidator<LookupTaxCodeQuery>
    {
        public LookupTaxCodeQueryValidator()
        {
            //.Must(tc => Regex.IsMatch(tc, @"^\d{10}$") || Regex.IsMatch(tc, @"^\d{13}$") || Regex.IsMatch(tc, @"^\d{14}$"))
            RuleFor(x => x.TaxCode)
            .NotEmpty()
            .WithMessage("Mã số thuế phải gồm 10, 13 hoặc 14 chữ số.");
        }
    }
}
