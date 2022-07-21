using api_todo.Data.EF;
using api_todo.Data.Entities;
using api_todo.Models.TodoChilds;
using api_todo.Models.TodoChildsl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_todo.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TodoChildsController : AuthController
{

    public TodoChildsController(TodoContext context, ILogger<TodosController> logger) : base(context, logger)
    {
    }

    [HttpGet("{Id}")]
    public IActionResult Get(int Id)
    {
        var todoChild = _context.TodoChilds.FirstOrDefault(x => x.Id == Id);
        return Ok(todoChild);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TodoChildCreate todoChildCreate)
    {
        if(!ModelState.IsValid)
        {
            return NotFound();
        }
        try{
            var todoChild = new TodoChild()
            { 
                Content = todoChildCreate.Content,
                TodoId = todoChildCreate.TodoId
            };
            var result = _context.Add(todoChild);
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
    public async Task<IActionResult> Update(int Id, [FromBody] TodoChildUpdate todoChildUpdate)
    {
        if (!ModelState.IsValid)
        {
            return NotFound();
        }
        var todoChild = _context.TodoChilds.FirstOrDefault(x => x.Id == Id);
        if (todoChild == null)
        {
            return NotFound("Todo is empty");
        }
        todoChild.Content = todoChildUpdate.Content;
        todoChild.Status = todoChildUpdate.Status;
        var result = _context.Update(todoChild);
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
        var todoChild = _context.TodoChilds.FirstOrDefault(x => x.Id == Id);
        if (todoChild == null)
        {
            return NotFound("Todo is empty");
        }
        var result = _context.Remove(todoChild);
        await _context.SaveChangesAsync();

        return Ok();
    }
}