namespace api_todo.Data.Entities;

public class Todo 
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public String Title { get; set; }
    public bool Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public User User { get; set; }
    public ICollection<TodoChild> TodoChilds { get; set; }
}