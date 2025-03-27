using Microsoft.AspNetCore.Identity;

namespace AuthControl.Domain.Entities;

public sealed class User : IdentityUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
}