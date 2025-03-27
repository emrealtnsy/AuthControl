namespace AuthControl.Application.Commands.Account.Login;

using FluentValidation;

public sealed class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username cannot be empty.")
            .MaximumLength(50).WithMessage("Username can be at most 50 characters long.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
    }
}