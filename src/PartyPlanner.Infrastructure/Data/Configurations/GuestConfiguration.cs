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
        builder.Property(guest => guest.Type).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(guest => guest.Status).HasMaxLength(40).IsRequired();
        builder.Property(guest => guest.InvitationToken).HasMaxLength(80).IsRequired();
        builder.Property(guest => guest.Email).HasMaxLength(160).IsRequired();
        builder.Property(guest => guest.PhoneNumber).HasMaxLength(30).IsRequired();
        builder.HasIndex(guest => guest.InvitationToken).IsUnique();
    }
}
