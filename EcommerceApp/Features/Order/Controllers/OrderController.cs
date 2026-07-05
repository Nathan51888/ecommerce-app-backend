using EcommerceApp.Features.Order.DTOs;
using EcommerceApp.Features.Order.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Features.Order.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
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
        return Ok();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderCreateRequestDto requestDto)
    {
        return Ok();
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateById([FromRoute] int id, [FromBody] OrderUpdateRequestDto requestDto)
    {
        return Ok();
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        return Ok();
    }
}