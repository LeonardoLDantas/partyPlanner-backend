using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Infrastructure.Data.Configurations;

public sealed class ConviteSenhaConfiguration : IEntityTypeConfiguration<EntityConviteSenha>
{
    public void Configure(EntityTypeBuilder<EntityConviteSenha> builder)
    {
        builder.ToTable("ConviteSenhas");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Codigo).HasMaxLength(20).IsRequired();
        builder.HasIndex(s => s.Codigo).IsUnique();
    }
}
