using AuthControl.Application.Commands.GenerateReferralLink.Event;
using AuthControl.Application.Common;
using AuthControl.Application.Common.Models;
using AuthControl.Infrastructure.Services;
using MediatR;

namespace AuthControl.Application.Commands.GenerateReferralLink;

public class GenerateReferralLinkCommandHandler(IMediator mediator, IReferralLinkService referralLinkService) :
    IRequestHandler<GenerateReferralLinkCommand, Result<Unit>>

{
    public async Task<Result<Unit>> Handle(GenerateReferralLinkCommand request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Email);

        var referralLink = await referralLinkService
            .CreateReferralLink(new ReferralLinkCreateRequestModel(Email: request.Email), cancellationToken);
      
        if (string.IsNullOrWhiteSpace(referralLink))
            return Result<Unit>.Failure("Referral link could not be created.");
        
        await mediator.Publish(new ReferralLinkGeneratedEvent(request.Email, referralLink), cancellationToken);
       return Result<Unit>.Success();
    }
}
