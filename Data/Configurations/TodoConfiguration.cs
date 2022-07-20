using api_todo.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_todo.Data.Configurations;

public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("Todos");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .UseIdentityColumn();

        builder.HasIndex(x => x.Title)
            .IsUnique();
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Status)
            .HasDefaultValue(false);    

        builder.Property(x => x.CreatedAt)
            .HasDefaultValue(DateTime.Now);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Todos)
            .HasForeignKey(x => x.UserId);
    }
}