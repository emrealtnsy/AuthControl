using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuthControl.Domain.Entities;
using AuthControl.Infrastructure.Configuration;
using AuthControl.Infrastructure.Helper;
using AuthControl.Infrastructure.Persistence.Contexts;
using AuthControl.Infrastructure.Persistence.Database;
using AuthControl.Infrastructure.Persistence.Database.Seed;
using AuthControl.Infrastructure.Persistence.Repository;
using AuthControl.Infrastructure.Services;
using AuthControl.WebAPI.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddDbContext<AuthControlDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IReferralLinkService, ReferralLinkService>();
builder.Services.AddScoped<ILoginAttemptService, LoginAttemptService>();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<ILinkProcessor, LinkProcessor>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IpResolver>();

builder.Services.AddScoped<DatabaseSetup>();
builder.Services.AddScoped<DatabaseSeeder>();
builder.Services.AddScoped<SeedData>();

builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddOptionalRateLimiter();

builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<AuthControlDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'");
    await next();
});

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy",
        "default-src 'self' http://localhost:4200 https://localhost:7050");
    await next();
});


app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.MapControllers();

using var scope = app.Services.CreateScope();
try
{
    await scope.ServiceProvider.GetRequiredService<DatabaseSeeder>().SeedAsync();
    Console.WriteLine("Database seeding completed successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Database seeding failed: {ex.Message}\nStack trace: {ex.StackTrace}");
    throw new InvalidOperationException("Database seeding failed.", ex);
}

app.Run();