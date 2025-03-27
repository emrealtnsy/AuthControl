namespace AuthControl.Infrastructure.Services;

public interface ILoginAttemptService
{
    Task HandleFailedLoginAttemptAsync(string userName, string ipAddress, CancellationToken cancellationToken);
    Task<bool> IsRequestLimitExceededAsync(string ipAddress, CancellationToken cancellationToken);
    Task<bool> IsBlockedAsync(string userName, string ipAddress, CancellationToken cancellationToken);
    Task ResetFailedAttemptsAsync(string userName, string ipAddress, CancellationToken cancellationToken);
}