using AuthControl.Application.Common.Models;
using AuthControl.Domain.Entities;
using AuthControl.Infrastructure.Configuration;
using AuthControl.Infrastructure.Helper;
using AuthControl.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthControl.Infrastructure.Services;

public class ReferralLinkService(IRepository<ReferralLink> referralLinkRepository,
    ILinkProcessor linkProcessor,IOptions<AppSettings> appSettings) : IReferralLinkService
{
    private readonly string _route = appSettings.Value.Route;
    private readonly int _referralLinkExpiredAt = appSettings.Value.ReferralLinkExpiresAt;
    public async Task<string> CreateReferralLink(ReferralLinkCreateRequestModel request, CancellationToken cancellationToken)
    {
        var token = linkProcessor.GenerateToken();
        var link = linkProcessor.CreateLink(_route, token);
        
        if(linkProcessor.IsValidUrl(link) is false)
            throw new Exception("Invalid link");
        
        await referralLinkRepository
            .AddAsync(new ReferralLink { Token = token, 
                ExpiresAt = DateTime.UtcNow.AddHours(_referralLinkExpiredAt) }, cancellationToken);
        
        await referralLinkRepository.SaveChangesAsync(cancellationToken);
        
        return token;
    }

    public async Task<bool> IsValidReferralLinkAsync(string token, CancellationToken cancellationToken)
    {
        return await referralLinkRepository.ExistsAsync(r =>
            r.Token == token &&
            r.ExpiresAt > DateTime.UtcNow &&
            !r.IsUsed, cancellationToken);
    }

    public async Task MarkReferralLinkAsUsedAsync(string token, CancellationToken cancellationToken)
    {
        await referralLinkRepository.Queryable()
            .Where(r => r.Token == token)
            .ExecuteUpdateAsync(p =>
                    p.SetProperty(x => x.IsUsed, true), cancellationToken);
    }
}