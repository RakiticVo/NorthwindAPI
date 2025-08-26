using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Infrastructure;
using NorthwindApi.Infrastructure.Locking;
using NorthwindApi.Infrastructure.Repository;
using NorthwindApi.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

// AppSettings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey))
        };
    });

builder.Services.AddAuthorization();

// EF Core
builder.Services.AddDbContext<NorthwindContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Northwind"));
});

// DI
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IDistributedLock>(sp =>
{
    var uow = sp.GetRequiredService<UnitOfWork>();
    return new SqlDistributedLock(uow);
});

builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NorthwindContext>();
    if (db.Database.CanConnect())
    {
        Console.WriteLine("✅ Database connection pre-warmed.");
    }
    else
    {
        Console.WriteLine("❌ Database connection failed to pre-warm.");
    }
}


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();