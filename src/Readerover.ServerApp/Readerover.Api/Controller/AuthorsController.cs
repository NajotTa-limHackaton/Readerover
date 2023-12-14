using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Readerover.Api.Common.Dtos;
using Readerover.Application.Common.Identity.Services;
using Readerover.Application.Common.Models.Querying;
using Readerover.Domain.Entities;
using Readerover.Infrastructure.Common.Extensions.Querying;

namespace Readerover.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController(IAuthorService authorService) : ControllerBase
{
    [HttpGet]
    public async ValueTask<IActionResult> Get([FromQuery] FilterPagination filterPagination)
    {
        var result = authorService.Get().ApplyPagination(filterPagination);
        return result.Any() ? Ok(result) : NoContent();
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await authorService.GetByIdAsync(id, cancellationToken: cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async ValueTask<IActionResult> Create([FromBody] AuthorDto authorDto, CancellationToken cancellationToken)
    {
        var result = await authorService.CreateAsync(authorDto.Adapt<Author>(), cancellationToken: cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async ValueTask<IActionResult> Update([FromBody] AuthorDto authorDto, CancellationToken cancellationToken)
    {
        var result = await authorService.UpdateAsync(authorDto.Adapt<Author>(), cancellationToken: cancellationToken);

        return result is not null ? Ok(result) : BadRequest();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async ValueTask<IActionResult> DeleteById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await authorService.DeleteByIdAsync(id, cancellationToken: cancellationToken);

        return result is not null ? Ok(result) : BadRequest();
    }

    [HttpDelete]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async ValueTask<IActionResult> Delete([FromBody]AuthorDto authorDto, CancellationToken cancellationToken)
    {
        var result = await authorService.DeleteAsync(authorDto.Adapt<Author>(), cancellationToken: cancellationToken);

        return result is not null ? Ok(result) : BadRequest();
    }

    [HttpPut("{authorId:guid}/uploadImage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async ValueTask<IActionResult> UploadImage([FromRoute]Guid authorId, IFormFile formFile, CancellationToken cancellationToken)
    {
        var result = await authorService.UploadImageAsync(authorId, formFile, cancellationToken: cancellationToken);

        return result is not null ? Ok(result) : BadRequest(result);
    }
}
