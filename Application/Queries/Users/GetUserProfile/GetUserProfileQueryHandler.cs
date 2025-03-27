using System.Security.Claims;
using AuthControl.Application.Common;
using AuthControl.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthControl.Application.Queries.Users.GetUserProfile;

public class GetUserProfileQueryHandler(UserManager<User> userManager,IHttpContextAccessor httpContextAccessor) 
    : IRequestHandler<GetUserProfileQuery, Result<GetUserProfileResponse>>
{
    public async Task<Result<GetUserProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await userManager.FindByIdAsync(userId);

        var roles = await userManager.GetRolesAsync(user);
        
        return Result<GetUserProfileResponse>
            .Success(new GetUserProfileResponse(
                Name : user.Name, 
                Surname : user.Surname, 
                UserName : user.UserName!, 
                Email : user.Email!, 
                Roles : roles 
            ));
    }
}