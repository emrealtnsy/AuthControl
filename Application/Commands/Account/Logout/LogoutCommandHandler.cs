using AuthControl.Application.Common;
using AuthControl.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthControl.Application.Commands.Account.Logout;

public class LogoutCommandHandler(SignInManager<User> signInManager) : IRequestHandler<LogoutCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await signInManager.SignOutAsync();
        return Result<string>.Success(string.Empty);
    }
}