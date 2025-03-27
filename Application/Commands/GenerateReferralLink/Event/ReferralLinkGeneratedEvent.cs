using MediatR;

namespace AuthControl.Application.Commands.GenerateReferralLink.Event;

public sealed record ReferralLinkGeneratedEvent(string Email, string ReferralLink) : INotification;
