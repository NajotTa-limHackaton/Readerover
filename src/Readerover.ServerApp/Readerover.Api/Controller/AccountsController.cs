using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Readerover.Api.Common.Dtos;
using Readerover.Application.Common.Identity.Services;
using Readerover.Application.Common.Models.Querying;
using Readerover.Domain.Common.Constants;
using Readerover.Domain.Entities;
using System.Text.Json;

namespace Readerover.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(IAccountService accountService) : ControllerBase
{
    [HttpGet]
    public ValueTask<IActionResult> Get([FromQuery] FilterPagination filterPagination)
    {
        var result = accountService.Get(filterPagination);

        return new(result.Any() ? Ok(result) : NoContent());
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await accountService.GetByIdAsync(id, cancellationToken: cancellationToken);

        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async ValueTask<IActionResult> Create([FromBody] UserDto userDto, CancellationToken cancellationToken)
    {
        var result = await accountService.CreateAsync(userDto.Adapt<User>(), cancellationToken: cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { Id = result.Id },
            result);
    }

    [HttpPut]
    public async ValueTask<IActionResult> Update([FromBody] UserDto userDto, CancellationToken cancellationToken)
    {
        var result = await accountService.UpdateAsync(userDto.Adapt<User>(), cancellationToken: cancellationToken);

        return result is not null ? Ok(result) : BadRequest();
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> DeleteById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await accountService.DeleteByIdAsync(id, cancellationToken: cancellationToken);

        return result is not null ? Ok(result) : BadRequest();
    }

    [Authorize]
    [HttpPost("uploadImage")]
    public async ValueTask<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken = default)
    {
        var userIdClaim = User.Claims.FirstOrDefault(claim => claim.Type == ClaimConstants.UserId)
            ?? throw new InvalidOperationException("User id not found!");
        var userId = Guid.Parse(userIdClaim.Value);

        var result = await accountService.UploadImageAsync(file, userId, cancellationToken);

        return result is not null ? Ok(result) : BadRequest();  
    }
}
