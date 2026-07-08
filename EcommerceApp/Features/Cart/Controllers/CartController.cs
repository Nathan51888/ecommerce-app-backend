using EcommerceApp.Features.Cart.DTOs;
using EcommerceApp.Features.Cart.Mappers;
using EcommerceApp.Features.Cart.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Features.Cart.Controllers;

[ApiController]
[Route("api/admin/carts")]
public sealed class CartController : ControllerBase
{
    private readonly ICartService _service;

    public CartController(ICartService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // TODO: Implement ASP.Identity to get user id
        var userId = 3;

        var items = await _service.GetAllAsync(userId);
        var response = items.Select(item => item.ToResponseDto());
        return Ok(response);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var userId = 3;

        var item = await _service.GetByIdAsync(userId, id);
        var response = item.ToResponseDto();
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CartItemCreateRequestDto requestDto)
    {
        var userId = 3;

        var createdItem = await _service.CreateAsync(userId, requestDto);
        var response = createdItem.ToResponseDto();
        return CreatedAtAction(nameof(GetById), new { id = createdItem.Id }, response);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateById([FromBody] CartItemUpdateRequestDto requestDto)
    {
        var userId = 3;

        var updatedItem = await _service.UpdateByIdAsync(userId, requestDto);
        var response = updatedItem.ToResponseDto();
        return Ok(response);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        var userId = 3;

        var deletedItem = await _service.DeleteByIdAsync(userId, id);

        return NoContent();
    }
}