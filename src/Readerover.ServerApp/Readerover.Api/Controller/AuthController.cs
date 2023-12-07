using Microsoft.AspNetCore.Mvc;
using Readerover.Application.Common.Identity.Services;
using Readerover.Application.Common.Models.Registering;

namespace Readerover.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("sign-up")]
    public async ValueTask<IActionResult> SignUp([FromBody]RegisterDetails registerDetails)
    {
        var result = await authService.SignUpAsync(registerDetails);

        return Ok(result);
    }

    [HttpPost("sign-in")]
    public async ValueTask<IActionResult> SignIn([FromBody] LoginDetails loginDetails)
    {
        var result = await authService.SignInAsync(loginDetails);

        return result is not null ? Ok(result) : BadRequest();
    }
}
