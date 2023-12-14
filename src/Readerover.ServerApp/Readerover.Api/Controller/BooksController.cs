using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Readerover.Api.Common.Dtos;
using Readerover.Application.Common.Identity.Services;
using Readerover.Application.Common.Models.Querying;
using Readerover.Domain.Common.Constants;
using Readerover.Domain.Entities;

namespace Readerover.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class BooksController(
    IBookOrchestrationService bookOrchestrationService,
    IBookService bookService)
    : ControllerBase
{
    [HttpGet]
    public ValueTask<IActionResult> Get([FromQuery] FilterPagination filterPagination)
    {
        var result = bookOrchestrationService.Get(filterPagination);

        return new(result.Any() ? Ok(result) : NoContent());
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await bookService.GetByIdAsync(id, cancellationToken: cancellationToken);

        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async ValueTask<IActionResult> Create([FromBody] BookDto bookDto, CancellationToken cancellationToken)
    {
        var result = await bookService.CreateAsync(bookDto.Adapt<Book>(), cancellationToken: cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async ValueTask<IActionResult> Update([FromBody] BookDto bookDto, CancellationToken cancellationToken)
    {
        var result = await bookService.UpdateAsync(bookDto.Adapt<Book>(), cancellationToken: cancellationToken);

        return result is not null ? Ok(result) : NoContent();
    }

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> DeleteById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await bookService.DeleteByIdAsync(id, cancellationToken: cancellationToken);

        return result is not null ? Ok(result) : BadRequest();
    }

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPut("uploadBookFile")]
    public async ValueTask<IActionResult> UploadBookFile(
        IFormFile book,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.Claims.FirstOrDefault(claim => claim.Type == ClaimConstants.UserId)
            ?? throw new InvalidOperationException("User id not found !");
        var userId = Guid.Parse(userIdClaim.Value);

        var result = await bookOrchestrationService.UploadBookFile(userId, book, cancellationToken);

        return result is not null ? Ok(result) : BadRequest();
    }

    [Authorize(Roles = "SuperAdmin,Admin")]
    [HttpPut("uploadBookAudio")]
    public async ValueTask<IActionResult> UploadBookAudio(
        IFormFile audio,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.Claims.FirstOrDefault(claim => claim.Type == ClaimConstants.UserId)
            ?? throw new InvalidOperationException("User id not found !");
        var userId = Guid.Parse(userIdClaim.Value);

        var result = await bookOrchestrationService.UploadBookAudio(userId, audio, cancellationToken);

        return result is not null ? Ok(result) : BadRequest();
    }
}
