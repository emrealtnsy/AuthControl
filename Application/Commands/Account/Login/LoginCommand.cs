using AuthControl.Application.Common;
using MediatR;

namespace AuthControl.Application.Commands.Account.Login;

public sealed record LoginCommand(string UserName, string Password) : IRequest<Result<string>>;