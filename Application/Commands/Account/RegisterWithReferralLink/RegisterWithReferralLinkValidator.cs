namespace AuthControl.Application.Commands.Account.RegisterWithReferralLink;

using FluentValidation;

public sealed class RegisterWithReferralLinkValidator : AbstractValidator<RegisterWithReferralLinkCommand>
{
    public RegisterWithReferralLinkValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .MaximumLength(50).WithMessage("Name can be at most 50 characters long.");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Surname cannot be empty.")
            .MaximumLength(50).WithMessage("Surname can be at most 50 characters long.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username cannot be empty.")
            .MaximumLength(50).WithMessage("Username can be at most 50 characters long.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Please enter a valid email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
    }
}