using EcommerceApp.Features.Product.Mappers;
using EcommerceApp.Features.Product.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Customer.Features.Product.Controllers;

[ApiController]
[Route("api/admin/products")]
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
}