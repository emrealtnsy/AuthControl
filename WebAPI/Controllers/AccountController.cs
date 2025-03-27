using AuthControl.Application.Queries.Users.GetUserProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthControl.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator mediator) : ControllerBase
    {
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var result = await mediator.Send(new GetUserProfileQuery());
            return result.IsSuccess ? Ok(result) : NotFound(result.Errors);
        }
    }
}