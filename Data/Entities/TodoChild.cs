namespace api_todo.Data.Entities;

public class TodoChild 
{
    public int Id { get; set; }
    public int TodoId { get; set; }
    public String Content { get; set; }
    public bool Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Todo Todo { get; set; }
}