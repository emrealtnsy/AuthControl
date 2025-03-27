using AuthControl.Application.Common.Models;

namespace AuthControl.Infrastructure.Services;

public interface IMailService
{ 
    Task<bool> SendEmailAsync(MailMessageModel mailMessage, CancellationToken cancellationToken = default);
}
