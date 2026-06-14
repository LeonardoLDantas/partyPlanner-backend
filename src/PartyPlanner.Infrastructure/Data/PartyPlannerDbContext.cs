using Microsoft.EntityFrameworkCore;
using PartyPlanner.Core.Entities;
using PartyPlanner.Infrastructure.Data.Configurations;

namespace PartyPlanner.Infrastructure.Data;

public sealed class PartyPlannerDbContext(DbContextOptions<PartyPlannerDbContext> options) : DbContext(options)
{
    public DbSet<EntityParty> Parties => Set<EntityParty>();
    public DbSet<EntityPartyTask> Tasks => Set<EntityPartyTask>();
    public DbSet<EntityGuest> Guests => Set<EntityGuest>();
    public DbSet<EntityAppNotification> AppNotifications => Set<EntityAppNotification>();
    public DbSet<EntityUser> Users => Set<EntityUser>();
    public DbSet<EntityUserExternalLogin> UserExternalLogins => Set<EntityUserExternalLogin>();

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
