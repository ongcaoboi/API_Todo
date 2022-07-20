namespace api_todo.Data.Entities;

public class User 
{
    public Guid Id { get; set; }
    public String Name { get; set; }
    public String Email { get; set; }
    public String Password { get; set; }
    public DateTime? CreatedAt { get; set; }
    
    public ICollection<Todo> Todos { get; set; }
}