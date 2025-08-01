using Applications.Features.ClientCredentials.Commands;
using Core.Enumerables;
using FluentValidation;

namespace Applications.Features.ClientCredentials.Validators
{
    public class CreateClientCredentialCommandValidator : AbstractValidator<CreateClientCredentialCommand>
    {
        public CreateClientCredentialCommandValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("ClientId is required.");

            RuleFor(x => x.ClientSecret)
                .NotEmpty().WithMessage("ClientSecret is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(x => x.Role)
                .Must(role => RoleEnum.All.Contains(role))
                .WithMessage($"Role must be one of: {string.Join(", ", RoleEnum.All)}");
        }
    }
}

