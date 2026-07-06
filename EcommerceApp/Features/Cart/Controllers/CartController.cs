using EcommerceApp.Features.Cart.DTOs;
using EcommerceApp.Features.Cart.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Features.Cart.Controllers;

[ApiController]
[Route("api/carts")]
public class CartController : ControllerBase
{
    private readonly ICartService _service;

    public CartController(ICartService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = 3;

        var items = await _service.GetAllAsync(userId);
        
        return Ok();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int itemId)
    {
        var userId = 3;

        var item = await _service.GetByIdAsync(userId, itemId);
        
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CartItemCreateRequestDto requestDto)
    {
        var userId = 3;

        var createdItem = await _service.CreateAsync(userId, requestDto);
        
        return Created();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateById([FromBody] CartItemUpdateRequestDto requestDto)
    {
        var userId = 3;

        var updatedItem = await _service.UpdateByIdAsync(userId, requestDto);
        
        return Ok();
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