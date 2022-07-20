using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using api_todo.Data.Entities;

namespace api_todo.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(x => x.Email)
            .IsUnique();
        builder.Property(x => x.Email)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasDefaultValue(DateTime.Now);
    }
}