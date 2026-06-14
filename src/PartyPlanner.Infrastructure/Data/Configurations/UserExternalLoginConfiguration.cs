using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartyPlanner.Core.Entities;

namespace PartyPlanner.Infrastructure.Data.Configurations;

public sealed class UserExternalLoginConfiguration : IEntityTypeConfiguration<EntityUserExternalLogin>
{
    public void Configure(EntityTypeBuilder<EntityUserExternalLogin> builder)
    {
        builder.ToTable("UserExternalLogins");
        builder.HasKey(login => login.Id);

        builder.Property(login => login.Provider).HasMaxLength(40).IsRequired();
        builder.Property(login => login.ProviderUserId).HasMaxLength(200).IsRequired();
        builder.Property(login => login.Email).HasMaxLength(160).IsRequired();
        builder.Property(login => login.LinkedAtUtc).IsRequired();

        builder.HasIndex(login => new { login.Provider, login.ProviderUserId }).IsUnique();
    }
}
