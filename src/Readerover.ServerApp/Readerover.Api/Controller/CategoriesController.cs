﻿using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Readerover.Api.Common.Dtos;
using Readerover.Application.Common.Identity.Services;
using Readerover.Application.Common.Models.Querying;
using Readerover.Domain.Entities;

namespace Readerover.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public ValueTask<IActionResult> Get([FromQuery] FilterPagination filterPagination)
    {
        var result = categoryService.Get(filterPagination);

        return new(result.Any() ? Ok(result) : NoContent());
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await categoryService.GetByIdAsync(id, cancellationToken: cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async ValueTask<IActionResult> Create([FromBody] CategoryDto categoryDto, CancellationToken cancellationToken)
    {
        var result = await categoryService.CreateAsync(categoryDto.Adapt<Category>(), cancellationToken: cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    [HttpPut]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async ValueTask<IActionResult> Update([FromBody] CategoryDto categoryDto, CancellationToken cancellationToken)
    {
        var result = await categoryService.UpdateAsync(categoryDto.Adapt<Category>(), cancellationToken: cancellationToken);
        return result is not null ? Ok(result) : BadRequest();
    }


    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await categoryService.DeleteByIdAsync(id, cancellationToken: cancellationToken);
        return result is not null ? Ok(result) : BadRequest();
    }
}
