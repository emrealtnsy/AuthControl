using AuthControl.Application.Commands.GenerateReferralLink;
using AuthControl.Application.Queries.Users.GetAllUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthControl.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminController(IMediator mediator) : ControllerBase
{
    [HttpPost("generateReferralLink")]
    public async Task<IActionResult> GenerateReferralLink([FromBody] GenerateReferralLinkCommand request)
    { 
        var result = await mediator.Send(request);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }
 
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllUsersQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }
} 