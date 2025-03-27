using AuthControl.Domain.Entities;

namespace AuthControl.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class LoginAttemptConfiguration : IEntityTypeConfiguration<LoginAttempt>
{
    public void Configure(EntityTypeBuilder<LoginAttempt> builder)
    {
        builder.ToTable("LoginAttempts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IpAddress)
            .IsRequired()
            .HasMaxLength(45); 

        builder.Property(x => x.Username)
            .HasMaxLength(100);
        
        builder.Property(x => x.AttemptTime)
            .IsRequired();
        
        builder.Property(x => x.BlockedUntil)
            .IsRequired(false);
    }
}