namespace AuthControl.Domain.Entities;

public sealed class LoginAttempt 
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string IpAddress { get; set; }
    public DateTime AttemptTime { get; set; }
    public DateTime? BlockedUntil { get; set; }
    public int AttemptCount { get; set; }
}