namespace AuthControl.Domain.Entities;

public sealed class ReferralLink
{
    public Guid Id { get; set; } 
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
}