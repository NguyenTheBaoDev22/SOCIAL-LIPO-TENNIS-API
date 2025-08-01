using Applications.Features.Users.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Users.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty().MinimumLength(3);
            RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^0\d{9}$").WithMessage("Invalid Vietnamese phone number");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }
}
