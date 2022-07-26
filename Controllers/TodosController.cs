using api_todo.Data.EF;
using api_todo.Data.Entities;
using api_todo.Models.Todos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_todo.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TodosController : AuthController
{

    public TodosController(TodoContext context, ILogger<TodosController> logger) : base(context, logger)
    {
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var todos = _context.Todos.Where(x => x.UserId == GetUserId()).ToList();
        return Ok(todos);
    }

    [HttpGet("{Id}")]
    public IActionResult Get(int Id)
    {
        var todoChilds = _context.TodoChilds.Where(x => x.TodoId == Id);
        return Ok(todoChilds);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TodoCreate todoCreate)
    {
        if(!ModelState.IsValid)
        {
            return NotFound();
        }
        try{
            var todo = new Todo()
            { 
                Title = todoCreate.Title,
                UserId = GetUserId(),
            };
            var result = _context.Add(todo);
            _logger.LogInformation("____________" + result);
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> Update(int Id, [FromBody] TodoUpdate todoUpdate)
    {
        if (!ModelState.IsValid)
        {
            return NotFound();
        }
        var todo = _context.Todos.FirstOrDefault(x => x.Id == Id);
        if (todo == null)
        {
            return NotFound("Todo is empty");
        }
        todo.Title = todoUpdate.Title;
        todo.Status = todoUpdate.Status;
        var result = _context.Update(todo);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        if (!ModelState.IsValid)
        {
            return NotFound();
        }
        var todo = _context.Todos.FirstOrDefault(x => x.Id == Id);
        if (todo == null)
        {
            return NotFound("Todo is empty");
        }
        var result = _context.Remove(todo);
        await _context.SaveChangesAsync();

        return Ok();
    }
}