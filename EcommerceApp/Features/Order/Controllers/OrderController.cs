using EcommerceApp.Features.Order.DTOs;
using EcommerceApp.Features.Order.Mappers;
using EcommerceApp.Features.Order.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Features.Order.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrderController : ControllerBase
{
    private readonly IOrderService _service;

    public OrderController(IOrderService service)
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
    public async Task<IActionResult> Create([FromBody] OrderCreateRequestDto requestDto)
    {
        var createdItem = await _service.CreateAsync(requestDto);
        var response = createdItem.ToResponseDto();
        return CreatedAtAction(nameof(GetById), new {id = createdItem.Id}, response);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateById([FromRoute] int id, [FromBody] OrderUpdateRequestDto requestDto)
    {
        var updated = await _service.UpdateByIdAsync(id, requestDto);
        return Ok(updated);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        var deleted = await _service.DeleteByIdAsync(id);
        return NoContent();
    }
}