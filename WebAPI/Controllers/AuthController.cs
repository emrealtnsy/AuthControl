using MediatR;
using Microsoft.AspNetCore.Mvc;
using AuthControl.Application.Commands.Account.Login;
using AuthControl.Application.Commands.Account.Logout;
using AuthControl.Application.Commands.Account.Register;
using AuthControl.Application.Commands.Account.RegisterWithReferralLink;
using Microsoft.AspNetCore.RateLimiting;

namespace AuthControl.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    [EnableRateLimiting("login-rate-limit")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(new { Token = result.Value }) : Unauthorized(result.Value);
    }

    [HttpPost("register")]
    [EnableRateLimiting("register-rate-limit")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPost("register/{token}")]
    [EnableRateLimiting("register-rate-limit")]
    public async Task<IActionResult> Register(string token, [FromBody] RegisterWithReferralLinkCommand request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await mediator.Send(new LogoutCommand());
        return Ok();
    }
}