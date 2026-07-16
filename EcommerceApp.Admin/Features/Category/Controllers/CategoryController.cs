using EcommerceApp.Features.Category.DTOs;
using EcommerceApp.Features.Category.Mappers;
using EcommerceApp.Features.Category.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Features.Category.Controllers;

[Route("api/admin/categories")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.GetAllAsync();
        var response = items.Select(item => item.ToResponseDto()).ToList();
        return Ok(response);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var item = await _service.GetByIdAsync(id);
        var response = item.ToResponseDto();
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateRequestDto requestDto)
    {
        var createdItem = await _service.CreateAsync(requestDto);
        var response = createdItem.ToResponseDto();
        return CreatedAtAction(nameof(GetById), new { id = createdItem.Id }, response);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateById([FromRoute] int id, [FromBody] CategoryUpdateRequestDto requestDto)
    {
        var updatedItem = await _service.UpdateByIdAsync(id, requestDto);
        return Ok(updatedItem);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        var deletedItem = await _service.DeleteByIdAsync(id);
        return NoContent();
    }
}