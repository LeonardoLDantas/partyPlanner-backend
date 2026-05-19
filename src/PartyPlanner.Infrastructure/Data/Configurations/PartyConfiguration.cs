using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Infrastructure.Data.Configurations;

public sealed class PartyConfiguration : IEntityTypeConfiguration<Party>
{
    public void Configure(EntityTypeBuilder<Party> builder)
    {
        builder.ToTable("Parties");
        builder.HasKey(party => party.Id);

        builder.Property(party => party.OwnerUserId).IsRequired();
        builder.Property(party => party.Name).HasMaxLength(150).IsRequired();
        builder.Property(party => party.Category).HasConversion<int>().IsRequired();
        builder.Property(party => party.Date).HasMaxLength(80).IsRequired();
        builder.Property(party => party.Time).HasMaxLength(20).IsRequired();
        builder.Property(party => party.Location).HasMaxLength(150).IsRequired();
        builder.Property(party => party.CoverImageUrl).HasColumnType("text").IsRequired();
        builder.Property(party => party.ExpectedGuests).IsRequired();
        builder.Property(party => party.IsFinalized).IsRequired();

        builder.HasMany(party => party.Tasks)
            .WithOne()
            .HasForeignKey("PartyId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(party => party.Guests)
            .WithOne()
            .HasForeignKey("PartyId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(party => party.Budget, budget =>
        {
            budget.Property(current => current.Estimated)
                .HasColumnName("BudgetEstimated")
                .HasColumnType("decimal(18,2)");

            budget.Property(current => current.Spent)
                .HasColumnName("BudgetSpent")
                .HasColumnType("decimal(18,2)");

            budget.OwnsMany(current => current.Items, item =>
            {
                item.ToTable("BudgetItems");
                item.WithOwner().HasForeignKey("PartyId");
                item.HasKey(current => current.Id);
                item.Property(current => current.Label).HasMaxLength(150).IsRequired();
                item.Property(current => current.Category).HasConversion<string>().HasMaxLength(80).IsRequired();
                item.Property(current => current.Amount).HasColumnType("decimal(18,2)");
            });
        });
    }
}
