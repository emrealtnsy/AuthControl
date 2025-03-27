namespace AuthControl.Application.Commands.GenerateReferralLink;

using FluentValidation;

public sealed class GenerateReferralLinkValidator : AbstractValidator<GenerateReferralLinkCommand>
{
    public GenerateReferralLinkValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Please enter a valid email address.");
    }
}