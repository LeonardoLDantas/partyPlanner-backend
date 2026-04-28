using Microsoft.EntityFrameworkCore;
using PartyPlanner.Core.Entities;
using PartyPlanner.Infrastructure.Data.Configurations;

namespace PartyPlanner.Infrastructure.Data;

public sealed class PartyPlannerDbContext(DbContextOptions<PartyPlannerDbContext> options) : DbContext(options)
{
    public DbSet<Party> Parties => Set<Party>();
    public DbSet<PartyTask> Tasks => Set<PartyTask>();
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<AppNotification> AppNotifications => Set<AppNotification>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserExternalLogin> UserExternalLogins => Set<UserExternalLogin>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PartyConfiguration());
        modelBuilder.ApplyConfiguration(new PartyTaskConfiguration());
        modelBuilder.ApplyConfiguration(new GuestConfiguration());
        modelBuilder.ApplyConfiguration(new AppNotificationConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserExternalLoginConfiguration());
    }
}
