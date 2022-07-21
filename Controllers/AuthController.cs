using System.Security.Claims;
using api_todo.Data.EF;
using api_todo.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace api_todo.Controllers;

public class AuthController : ControllerBase
{
    protected readonly TodoContext _context;
    protected readonly ILogger<AuthController> _logger;

    public AuthController(TodoContext context, ILogger<AuthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    protected Guid GetUserId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userClaims = identity.Claims;

        var Id = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        return new Guid(Id);
    }

    protected User GetCurrentUser()
    {
        return _context.Users.FirstOrDefault(x => x.Id == GetUserId());
    }
}