using api_todo.Data.Configurations;
using api_todo.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace api_todo.Data.EF;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TodoConfiguration());
        modelBuilder.ApplyConfiguration(new TodoChildConfiguration());

        modelBuilder.Entity<User>().HasData(
            new User() {
                Id = new Guid("9E009A8E-162A-43D8-94C3-E0E9B9DB0B43"),
                Name = "user",
                Email = "user@gmail.com",
                Password = "123456"
            }
        );
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Todo> Todos { get; set; }
    public DbSet<TodoChild> TodoChilds { get; set; }
}