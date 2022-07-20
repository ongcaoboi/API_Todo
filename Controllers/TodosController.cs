using api_todo.Data.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_todo.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TodosController : ControllerBase
{
    private readonly TodoContext _context;
    private readonly ILogger<TodosController> _logger;

    public TodosController(TodoContext context, ILogger<TodosController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Hi()
    {
        return Content("Wellcome to Todo controller");
    }
}