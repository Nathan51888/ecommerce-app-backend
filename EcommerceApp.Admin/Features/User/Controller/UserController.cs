using EcommerceApp.Features.User.DTOs;
using EcommerceApp.Features.User.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Features.User.Controller;

[ApiController]
[Route("api/admin/auth")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequestDto requestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest("Something went wrong");

        var result = await _service.LoginUser(requestDto);
        if (!result)
            return BadRequest("Something went wrong");

        var tokenString = _service.GenerateTokenString(requestDto);
        return Ok(tokenString);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto requestDto)
    {
        var result = await _service.RegisterUser(requestDto);
        if (!result)
            return BadRequest("Something went wrong");

        return Ok("Register success");
    }
}