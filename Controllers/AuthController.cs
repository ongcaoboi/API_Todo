using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_todo.Data.EF;
using api_todo.Data.Entities;
using api_todo.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace api_todo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly TodoContext _context;
    public AuthController(IConfiguration config, TodoContext context)
    {
        _config = config;
        _context = context;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login([FromBody] UserLogin userLogin)
    {
        if (!ModelState.IsValid)
        {
            return NotFound();
        }

        var user = _context.Users.FirstOrDefault(
            x => x.Email.ToLower() == userLogin.Email.ToLower() &&
                x .Password == userLogin.Password
            );

        if (user == null)
        {
            return NotFound("User not found");
        }

        var token = GenerateToken(user);
        return Ok(token);
    }
    
    private string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new []
        {
            new Claim(ClaimTypes.NameIdentifier, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}