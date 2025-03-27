using AuthControl.Application.Common.Models;

namespace AuthControl.Infrastructure.Services;

public interface IReferralLinkService
{
    Task<string> CreateReferralLink(ReferralLinkCreateRequestModel request, CancellationToken cancellationToken = default);
    Task<bool> IsValidReferralLinkAsync(string token, CancellationToken cancellationToken = default);
    Task MarkReferralLinkAsUsedAsync(string token, CancellationToken cancellationToken = default);
}