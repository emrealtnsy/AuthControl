using AuthControl.Application.Common.Models;
using AuthControl.Infrastructure.Services;
using MediatR;

namespace AuthControl.Application.Commands.GenerateReferralLink.Event;

public class ReferralLinkGeneratedEventHandler(IMailService mailService)
    : INotificationHandler<ReferralLinkGeneratedEvent>
{

    public async Task Handle(ReferralLinkGeneratedEvent notification, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification);
        ArgumentException.ThrowIfNullOrWhiteSpace(notification.Email);
        
           var mailMessage = new MailMessageModel(
                Subject : "Your Referral Link",
                Body : $"Click here to register: {notification.ReferralLink}",
                To :  notification.Email,
                IsBodyHtml : false
            );
        
           await mailService.SendEmailAsync(mailMessage, cancellationToken).ConfigureAwait(false);
    }
}