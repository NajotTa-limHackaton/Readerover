using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Readerover.Api.Common.Dtos;
using Readerover.Application.Common.Identity.Services;
using Readerover.Application.Common.Models.Querying;
using Readerover.Domain.Entities;

namespace Readerover.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class SubCategoriesController(ISubCategoryService subCategoryService) : ControllerBase
{
    [HttpGet]
    public ValueTask<IActionResult> Get([FromQuery] FilterPagination filterPagination)
    {
        var result = subCategoryService.Get(filterPagination);

        return new(result.Any() ? Ok(result) : NoContent());
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await subCategoryService.GetByIdAsync(id, cancellationToken: cancellationToken);

        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async ValueTask<IActionResult> Create([FromBody] SubCategoryDto subCategoryDto, CancellationToken cancellationToken)
    {
        var result = await subCategoryService.CreateAsync(subCategoryDto.Adapt<SubCategory>(), cancellationToken: cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async ValueTask<IActionResult> Update([FromBody] SubCategoryDto subCategoryDto, CancellationToken cancellationToken = default)
    {
        var result = await subCategoryService.UpdateAsync(subCategoryDto.Adapt<SubCategory>(), cancellationToken: cancellationToken);

        return result is not null ? Ok(result) : BadRequest();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async ValueTask<IActionResult> DeleteById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await subCategoryService.DeleteByIdAsync(id, cancellationToken: cancellationToken);

        return result is not null ? Ok(result) : BadRequest();
    }
}
