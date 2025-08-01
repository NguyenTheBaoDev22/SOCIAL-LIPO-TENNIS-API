using Applications.Features.Shops.Inventories.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Shops.Products.Validators
{
    public class CreateShopInventoryAuditCommandValidator : AbstractValidator<CreateShopInventoryCommand>
    {
        public CreateShopInventoryAuditCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.AuditDate).NotEmpty().LessThanOrEqualTo(DateTime.Now);
            RuleFor(x => x.ExpectedQty).GreaterThanOrEqualTo(0);
            RuleFor(x => x.ActualQty).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Reason).MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Reason));
            RuleFor(x => x.AuditorName).MaximumLength(255).When(x => !string.IsNullOrEmpty(x.AuditorName));
        }
    }
}
