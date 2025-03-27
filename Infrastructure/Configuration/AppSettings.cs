namespace AuthControl.Infrastructure.Configuration;

public sealed record AppSettings
{
    public required string Route { get; init; }
    public int ReferralLinkExpiresAt { get; init; }
    public required MailConfig MailConfig { get; init; }
    public required LoginSettings LoginSettings { get; init; }
    public required JwtSettings JwtSettings { get; init; }
}

public sealed class MailConfig
{
    public required string SmtpHost { get; init; }
    public required int SmtpPort { get; init; }
    public required string SmtpUser { get; init; }
    public required string SmtpPass { get; init; }
    public required bool EnableSsl { get; init; } 
    public required bool UseDefaultCredentials { get; init; }
    public required string FromEmail { get; init; }
}

public sealed class LoginSettings
{
    public required int MaxFailedAttempts { get; init; }
    public required int MaxRequestsPerMinute { get; init; }
    private double BlockTimeMinutes { get; init; }
    public TimeSpan BlockTime => TimeSpan.FromMinutes(BlockTimeMinutes);
}

public sealed record JwtSettings
{  
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string Key { get; init; }
    public required int Expires { get; init; }
}
