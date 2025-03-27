using AuthControl.Domain.Entities;
using AuthControl.Infrastructure.Configuration;
using AuthControl.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthControl.Infrastructure.Services;

public class LoginAttemptService(IRepository<LoginAttempt> loginAttemptRepository,
    IOptions<AppSettings> appSettings) : ILoginAttemptService
{
    private readonly LoginSettings _loginSettings = appSettings.Value.LoginSettings;

    public async Task HandleFailedLoginAttemptAsync(string userName, string ipAddress,
        CancellationToken cancellationToken)
    {
        var existingAttempt = await loginAttemptRepository
            .FindAsync(a => a.Username == userName && a.IpAddress == ipAddress,trackChanges: true, cancellationToken);
        
        if (existingAttempt is null)
        {
            existingAttempt = new LoginAttempt
            {
                Username = userName,
                IpAddress = ipAddress
            };
            
            await loginAttemptRepository.AddAsync(existingAttempt, cancellationToken);
        }

        existingAttempt.AttemptCount += 1;
        existingAttempt.AttemptTime = DateTime.UtcNow;

        if (existingAttempt.AttemptCount >= _loginSettings.MaxFailedAttempts)
            existingAttempt.BlockedUntil = DateTime.UtcNow.AddMinutes(_loginSettings.BlockTimeMinutes);

        await loginAttemptRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IsBlockedAsync(string userName, string ipAddress, CancellationToken cancellationToken)
        => await loginAttemptRepository
            .ExistsAsync(a => a.IpAddress == ipAddress && a.Username == userName &&
                              a.BlockedUntil > DateTime.UtcNow, cancellationToken);

    public async Task ResetFailedAttemptsAsync(string userName, string ipAddress, CancellationToken cancellationToken)
        => await loginAttemptRepository.Queryable()
            .Where(a => a.Username == userName && a.IpAddress == ipAddress)
            .ExecuteDeleteAsync(cancellationToken);
    
}