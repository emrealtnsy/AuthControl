using AuthControl.Application.Common;
using MediatR;

namespace AuthControl.Application.Commands.Account.Register;

public sealed record RegisterCommand(string Name, 
    string Surname, 
    string UserName,
    string Email,
    string Password) : IRequest<Result<Unit>>;