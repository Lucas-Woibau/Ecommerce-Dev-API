using EcommerceDev.Application.Commands.Auth.Login;
using EcommerceDev.Application.Commands.Auth.Register;
using EcommerceDev.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceDev.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand request)
    {
        var result = await _mediator
            .DispatchAsync<RegisterCommand, ResultViewModel<Guid>>(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand request)
    {
        var result = await _mediator
            .DispatchAsync<LoginCommand, ResultViewModel<LoginResponse>>(request);

        if (!result.IsSuccess)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }
}
