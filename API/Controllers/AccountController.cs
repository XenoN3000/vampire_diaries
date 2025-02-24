using System.Security.Claims;
using API.DTOs;
using API.Helpers;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly TokenService _tokenService;

    public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }


    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.DeviceId == loginDto.DeviceId);

        if (user == null) return await Register(new RegisterDto { DeviceId = loginDto.DeviceId });

        return UserDto.CreateFromUser(user, _tokenService.CreateToken(user));
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
    {
        if (await _userManager.Users.AnyAsync(u => u.DeviceId == registerDto.DeviceId))
        {
            ModelState.AddModelError("DeviceId", "DeviceId is taken before");
            return ValidationProblem();
        }

        var user = new AppUser
        {
            DeviceId = registerDto.DeviceId,
        };

        user.UserName = $"Guest-{user.Id}";

        var result = await _userManager.CreateAsync(user);

        if (result.Succeeded)
        {
            return UserDto.CreateFromUser(user, _tokenService.CreateToken(user));
        }

        return BadRequest(result.Errors);
    }


    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u =>
            u.DeviceId == User.FindFirstValue(Konstants.ClaimTypes.DeviceId.ToString()));

        return UserDto.CreateFromUser(user, _tokenService.CreateToken(user));
    }
}