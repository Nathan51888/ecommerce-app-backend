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
        return Ok();
    }
}