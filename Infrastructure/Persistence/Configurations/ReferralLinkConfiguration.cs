using AuthControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthControl.Infrastructure.Persistence.Configurations;

public sealed class ReferralLinkConfiguration : IEntityTypeConfiguration<ReferralLink>
{
    public void Configure(EntityTypeBuilder<ReferralLink> builder)
    {
        builder.ToTable("ReferralLinks");

        builder.HasKey(rl => rl.Id);
        
        builder.Property(rl => rl.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(rl => rl.ExpiresAt)
            .IsRequired();
        
        builder.Property(rl => rl.IsUsed)
            .IsRequired()
            .HasDefaultValue(false);
        
    }
}
