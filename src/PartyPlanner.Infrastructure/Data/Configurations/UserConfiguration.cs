using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Infrastructure.Data.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<EntityUser>
{
    public void Configure(EntityTypeBuilder<EntityUser> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Name).HasMaxLength(120).IsRequired();
        builder.Property(user => user.Email).HasMaxLength(160).IsRequired();
        builder.Property(user => user.PasswordHash).HasMaxLength(400).IsRequired();
        builder.Property(user => user.CreatedAtUtc).IsRequired();
        builder.HasIndex(user => user.Email).IsUnique();

        builder.HasMany(user => user.Parties)
            .WithOne(party => party.Owner)
            .HasForeignKey(party => party.OwnerUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(user => user.ExternalLogins)
            .WithOne(login => login.EntityUser)
            .HasForeignKey(login => login.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(user => user.Notifications)
            .WithOne(notification => notification.EntityUser)
            .HasForeignKey(notification => notification.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
