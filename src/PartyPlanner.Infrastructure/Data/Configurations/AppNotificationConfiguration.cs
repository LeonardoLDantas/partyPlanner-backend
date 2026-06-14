using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Infrastructure.Data.Configurations;

public sealed class AppNotificationConfiguration : IEntityTypeConfiguration<EntityAppNotification>
{
    public void Configure(EntityTypeBuilder<EntityAppNotification> builder)
    {
        builder.ToTable("AppNotifications");
        builder.HasKey(notification => notification.Id);

        builder.Property(notification => notification.Title).HasMaxLength(120).IsRequired();
        builder.Property(notification => notification.Message).HasMaxLength(500).IsRequired();
        builder.Property(notification => notification.Type).HasMaxLength(40).IsRequired();
        builder.Property(notification => notification.CreatedAtUtc).IsRequired();
        builder.Property(notification => notification.IsRead).IsRequired();

        builder.HasIndex(notification => new { notification.UserId, notification.CreatedAtUtc });
    }
}
