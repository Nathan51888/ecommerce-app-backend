using EcommerceApp.Features.Product.DTOs;
using EcommerceApp.Features.Product.Mappers;
using EcommerceApp.Features.Product.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Features.Product.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.GetAllAsync();
        var itemsDto = items.Select(item => item.ToResponseDto()).ToList();
        return Ok(itemsDto);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var item = await _service.GetByIdAsync(id);
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateRequestDto requestDto)
    {
        var item = await _service.CreateAsync(requestDto);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item.ToResponseDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateById([FromRoute] int id, [FromBody] ProductUpdateRequestDto requestDto)
    {
        var item = await _service.UpdateAsync(requestDto);
        return Ok(item);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        var item = await _service.DeleteByIdAsync(id);
        return NoContent();
    }
}