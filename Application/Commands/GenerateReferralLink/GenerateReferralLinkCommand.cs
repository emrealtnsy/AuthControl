using AuthControl.Application.Common;
using MediatR;

namespace AuthControl.Application.Commands.GenerateReferralLink;

public sealed record GenerateReferralLinkCommand(string Email) : IRequest<Result<Unit>>;
