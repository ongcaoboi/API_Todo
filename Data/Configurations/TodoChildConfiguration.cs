using api_todo.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_todo.Data.Configurations;

public class TodoChildConfiguration : IEntityTypeConfiguration<TodoChild>
{
    public void Configure(EntityTypeBuilder<TodoChild> builder)
    {
        builder.ToTable("TodoChilds");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseIdentityColumn();

        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Status)
            .HasDefaultValue(false);    

        builder.Property(x => x.CreatedAt)
            .HasDefaultValue(DateTime.Now);

        builder.HasOne(x => x.Todo)
            .WithMany(x => x.TodoChilds)
            .HasForeignKey(x => x.TodoId);
    }
}