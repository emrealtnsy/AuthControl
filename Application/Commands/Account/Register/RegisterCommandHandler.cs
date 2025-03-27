using AuthControl.Application.Common;
using AuthControl.Application.Common.Models;
using AuthControl.Domain.Constants;
using AuthControl.Infrastructure.Services;
using MediatR;

namespace AuthControl.Application.Commands.Account.Register;

public class RegisterCommandHandler(IRegisterService registerService)
    : IRequestHandler<RegisterCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var checkEmail =  await registerService.CheckEmailExistsAsync(request.Email, cancellationToken);
        if (checkEmail)
            return  Result<Unit>.Failure("Email already exists" );

        var result =  await registerService
            .CreateAsync(new AccountCreateRequestModel(
                UserName: request.UserName, 
                Email: request.Email, 
                Name: request.Name, 
                Surname: request.Surname, 
                Password: request.Password, 
                Role: RoleConstants.Customer), cancellationToken);
        
        return !result.Succeeded ? Result<Unit>.Failure(result.Errors) : Result<Unit>.Success();
    }
}

