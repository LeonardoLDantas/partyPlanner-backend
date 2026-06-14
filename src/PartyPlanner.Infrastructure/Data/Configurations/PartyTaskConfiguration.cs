using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Infrastructure.Data.Configurations;

public sealed class PartyTaskConfiguration : IEntityTypeConfiguration<EntityPartyTask>
{
    public void Configure(EntityTypeBuilder<EntityPartyTask> builder)
    {
        builder.ToTable("Tasks");
        builder.HasKey(task => task.Id);
        builder.Property(task => task.Title).HasMaxLength(150).IsRequired();
        builder.Property(task => task.Assignee).HasMaxLength(120).IsRequired();
        builder.Property(task => task.DueDate).HasMaxLength(40).IsRequired();
        builder.Property(task => task.Description).HasMaxLength(500).IsRequired();
        builder.Property(task => task.Status).HasMaxLength(40).IsRequired();
    }
}
