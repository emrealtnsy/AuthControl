using AuthControl.Application.Common;
using MediatR;

namespace AuthControl.Application.Commands.Account.RegisterWithReferralLink;

public sealed record RegisterWithReferralLinkCommand(
    string Name, 
    string Surname, 
    string UserName,
    string Email,
    string Password, 
    string Token) : IRequest<Result<Unit>>;
    