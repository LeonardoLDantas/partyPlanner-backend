using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Infrastructure.Data.Configurations;

public sealed class ConviteConfiguration : IEntityTypeConfiguration<EntityConvite>
{
    public void Configure(EntityTypeBuilder<EntityConvite> builder)
    {
        builder.ToTable("Convites");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nome).HasMaxLength(150).IsRequired();
        builder.Property(c => c.Observacao).HasMaxLength(500).IsRequired();
        builder.Property(c => c.Tipo).HasConversion<int>().IsRequired();
        builder.Property(c => c.SenhaPresente).HasMaxLength(80).IsRequired();
        builder.Property(c => c.CreatedAt).IsRequired();

        builder.HasMany(c => c.Senhas)
            .WithOne()
            .HasForeignKey("ConviteId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Guests)
            .WithOne()
            .HasForeignKey("ConviteId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
