using Applications.Features.AppAccounts.Commands;
using FluentValidation;

namespace Applications.Features.AppAccounts.Validators;

/// <summary>
/// Validator cho SetupPasswordCommand.
/// </summary>
public class SetupPasswordCommandValidator : AbstractValidator<SetupPasswordCommand>
{
    public SetupPasswordCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token không được để trống.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Mật khẩu không được để trống.")
            .MinimumLength(8).WithMessage("Mật khẩu phải có ít nhất 8 ký tự.")
            .Matches("[A-Z]").WithMessage("Mật khẩu phải có ít nhất một chữ in hoa.")
            .Matches("[a-z]").WithMessage("Mật khẩu phải có ít nhất một chữ thường.")
            .Matches("[0-9]").WithMessage("Mật khẩu phải có ít nhất một số.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Mật khẩu phải có ít nhất một ký tự đặc biệt.");
    }
}
