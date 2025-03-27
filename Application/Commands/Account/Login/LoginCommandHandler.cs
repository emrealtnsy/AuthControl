using AuthControl.Application.Common;
using AuthControl.Domain.Entities;
using AuthControl.Infrastructure.Helper;
using AuthControl.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthControl.Application.Commands.Account.Login;

public class LoginCommandHandler(SignInManager<User> signInManager, UserManager<User> userManager,
    ILoginAttemptService loginAttemptService, IJwtTokenService jwtTokenService, 
    IpResolver ipResolver) : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var ipAddress = ipResolver.GetIpAddress();

        var user = await userManager.FindByNameAsync(request.UserName);
        var roles = await userManager.GetRolesAsync(user!);


        if (await loginAttemptService.IsRequestLimitExceededAsync(ipAddress!, cancellationToken))
        {
            return Result<string>.Failure("Too many requests. Please try again in 1 minute.");
        }
       
        if (await loginAttemptService.IsBlockedAsync(user.UserName, ipAddress!, cancellationToken))
            return Result<string>.
                Failure("Too many failed login attempts. Your account is temporarily blocked. Please try again later.");
        
        var result = await signInManager.PasswordSignInAsync(
            user, request.Password, false, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            await loginAttemptService
                .HandleFailedLoginAttemptAsync(user.UserName, ipAddress!, cancellationToken);
            return Result<string>.Failure();
        }

        await loginAttemptService
            .ResetFailedAttemptsAsync(request.UserName, ipAddress!, cancellationToken);

        var jwtToken = jwtTokenService.GenerateToken(user, roles);
        return Result<string>.Success(jwtToken);
    }
}