using AuthControl.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthControl.Infrastructure.Services;

public interface IRegisterService
{ 
    Task<IdentityResult> CreateAsync(AccountCreateRequestModel model, CancellationToken cancellationToken = default); 
    Task<bool> CheckEmailExistsAsync(string email, CancellationToken cancellationToken = default);
}