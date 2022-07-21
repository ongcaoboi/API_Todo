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

[Route("api")]
[ApiController]
public class UserController : AuthController
{
    private readonly IConfiguration _config;
    public UserController(TodoContext context, ILogger<UserController> logger, IConfiguration config)
    : base(context, logger)
    {
        _config = config;
    }

    [AllowAnonymous]
    [HttpPost("login")]
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

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
    {
        if (!ModelState.IsValid)
        {
            return NotFound();
        }

        var userTmp = _context.Users.FirstOrDefault(x => x.Email.ToLower() == userRegister.Email.ToLower());

        if (userTmp != null)
        {
            return BadRequest("Email already used!");
        }

        User user = new User()
        {
            Email = userRegister.Email,
            Name = userRegister.Name,
            Password = userRegister.Password,
        };

        var ex = _context.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Successful registration, please login to use!");
    }

    [Authorize]
    [HttpPut("changepassword")]
    public async Task<IActionResult> ChangePassword([FromBody] UserChangePassword userChangePassword)
    {
        if (!ModelState.IsValid)
        {
            return NotFound();
        }

        var user = GetCurrentUser();
        
        if (userChangePassword.Password != user.Password)
        {
            return BadRequest("Incorrect password");
        }

        user.Password = userChangePassword.NewPassword;

        var result = _context.Update(user);
        await _context.SaveChangesAsync();

        return Ok("");
    }

    [HttpGet("user")]
    public IActionResult GetUserLogin()
    {
        var user = GetCurrentUser();
        return Ok(user);
    }
    
    private string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new []
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
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