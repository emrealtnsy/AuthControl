using System.Reflection;
using AuthControl.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthControl.Infrastructure.Persistence.Contexts;

public class AuthControlDbContext(DbContextOptions<AuthControlDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<LoginAttempt> LoginAttempts { get; set; }
    public DbSet<ReferralLink> ReferralLinks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}


