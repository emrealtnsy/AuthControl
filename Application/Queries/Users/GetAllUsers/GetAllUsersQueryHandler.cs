using AuthControl.Application.Common;
using AuthControl.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthControl.Application.Queries.Users.GetAllUsers;

public class GetAllUsersQueryHandler(UserManager<User> userManager) 
    : IRequestHandler<GetAllUsersQuery, Result<List<GetAllUserResponse>>>
{
    public async Task<Result<List<GetAllUserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userManager.Users.AsNoTracking().ToListAsync(cancellationToken);
        
        if (!users.Any())
            return Result<List<GetAllUserResponse>>.Failure("No users found.");

        var userResponses = new List<GetAllUserResponse>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);

            userResponses.Add(
                new GetAllUserResponse(
                Name : user.Name,
                Surname : user.Surname,
                UserName : user.UserName!,
                Email : user.Email!,
                EmailConfirmed : user.EmailConfirmed,
                Roles : roles
            ));
        }

        if (!userResponses.Any())
            return Result<List<GetAllUserResponse>>.Failure("No valid users found.");

        return Result<List<GetAllUserResponse>>.Success(userResponses);
    }
}