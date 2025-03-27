using AuthControl.Application.Common;
using AuthControl.Application.Common.Models;
using AuthControl.Domain.Constants;
using AuthControl.Infrastructure.Services;
using MediatR;

namespace AuthControl.Application.Commands.Account.RegisterWithReferralLink;

public class RegisterWithReferralLinkCommandHandler(IRegisterService registerService,
    IReferralLinkService referralLinkService) : IRequestHandler<RegisterWithReferralLinkCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(RegisterWithReferralLinkCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Password);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Token);

        var isValid = await referralLinkService.IsValidReferralLinkAsync(request.Token, cancellationToken);
        if (!isValid) 
            return Result<Unit>.Failure("Invalid link");

        var checkEmail =  await registerService.CheckEmailExistsAsync(request.Email, cancellationToken);
        if (checkEmail)
            return  Result<Unit>.Failure("Email already exists" );
        
        var result = await registerService
            .CreateAsync(new AccountCreateRequestModel(
                    UserName: request.UserName, 
                    Email: request.Email, 
                    Name: request.Name, 
                    Surname: request.Surname, 
                    Password: request.Password, 
                    Role: RoleConstants.Manager), cancellationToken);

        if (!result.Succeeded)
           return Result<Unit>.Failure(result.Errors);

        await referralLinkService.MarkReferralLinkAsUsedAsync(request.Token, cancellationToken);

        return Result<Unit>.Success();
    }
}