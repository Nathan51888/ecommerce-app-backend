using EcommerceApp.Features.Product.Mappers;
using EcommerceApp.Features.Products.DTOs;
using EcommerceApp.Features.Products.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Features.Product.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
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
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateRequestDto requestDto)
    {
        var item = await _service.CreateAsync(requestDto);
        return CreatedAtAction(nameof(GetById), new {id = item.Id}, item.ToResponseDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateById([FromRoute] int id)
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