using AuthControl.Application.Common.Models;
using AuthControl.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthControl.Infrastructure.Services;

public class RegisterService(UserManager<User> userManager) : IRegisterService
{
    public async Task<IdentityResult> CreateAsync(AccountCreateRequestModel model, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Name = model.Name,
            Surname = model.Surname,
            Email = model.Email,
            UserName = model.UserName,
        };
        
        var createResult = await userManager.CreateAsync(user, model.Password);
        if (!createResult.Succeeded)
            return IdentityResult.Failed(createResult.Errors.ToArray());

        var roleResult = await userManager.AddToRoleAsync(user, model.Role);
        return roleResult.Succeeded 
            ? IdentityResult.Success : IdentityResult.Failed(roleResult.Errors.ToArray());
    }
    
    public async Task<bool> CheckEmailExistsAsync(string email, CancellationToken cancellationToken) 
        =>  await userManager.Users
            .AnyAsync(u => u.Email == email, cancellationToken: cancellationToken);
}
