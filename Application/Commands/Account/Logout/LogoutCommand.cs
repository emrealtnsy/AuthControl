using AuthControl.Application.Common;
using MediatR;

namespace AuthControl.Application.Commands.Account.Logout;

public sealed record LogoutCommand : IRequest<Result<string>>;