using System.Net;
using System.Net.Mail;
using AuthControl.Application.Common.Models;
using AuthControl.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace AuthControl.Infrastructure.Services;

public class MailService(IOptions<AppSettings> appSettings) : IMailService
{
    private readonly MailConfig _mailConfig = appSettings.Value.MailConfig
                                              ?? throw new InvalidOperationException("MailConfig is null.");

    public async Task<bool> SendEmailAsync(MailMessageModel mailMessage, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(mailMessage);

        try
        {
            using var smtpClient = new SmtpClient(_mailConfig.SmtpHost);
            smtpClient.Port = _mailConfig.SmtpPort;
            smtpClient.Credentials = new NetworkCredential(_mailConfig.SmtpUser, _mailConfig.SmtpPass);
            smtpClient.EnableSsl = _mailConfig.EnableSsl;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = _mailConfig.UseDefaultCredentials;

            var message = new MailMessage(_mailConfig.FromEmail, mailMessage.To, mailMessage.Subject, mailMessage.Body)
            {
                IsBodyHtml = mailMessage.IsBodyHtml
            };

            await smtpClient.SendMailAsync(message, cancellationToken).ConfigureAwait(false);
            return true;
        }
        catch
        {
            return false;
        }
    }
}