using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Infrastructure.Data.Configurations;

public sealed class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.ToTable("Guests");
        builder.HasKey(guest => guest.Id);
        builder.Property(guest => guest.Name).HasMaxLength(150).IsRequired();
        builder.Property(guest => guest.Group).HasMaxLength(100).IsRequired();
        builder.Property(guest => guest.Status).HasMaxLength(40).IsRequired();
    }
}
