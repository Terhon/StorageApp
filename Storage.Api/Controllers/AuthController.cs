using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Storage.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration config, UserManager<IdentityUser> userManager) : ControllerBase
{
    public record LoginRequest(string Username, string Password);

    private record LoginResponse(string Token);

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var user = await userManager.FindByNameAsync(req.Username);
        if (user == null || !await userManager.CheckPasswordAsync(user, req.Password))
            return Unauthorized();

        var jwt = config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, req.Username),
            new Claim(ClaimTypes.Name, req.Username)
        };

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new LoginResponse(tokenString));
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(string login, string password)
    {
        var user = new IdentityUser(login);

        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
            return Ok("User created!");

        return BadRequest(result.Errors);
    }
}