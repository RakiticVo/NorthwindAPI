using Serilog;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NorthwindApi.Application;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.DateTimes;
using NorthwindApi.Application.Mapping;
using NorthwindApi.Infrastructure.Cache;
using NorthwindApi.Infrastructure.Locking;
using NorthwindApi.Infrastructure.Security;
using NorthwindApi.Infrastructure.Middlewares;
using NorthwindApi.Persistence;
using NorthwindApi.Persistence.Repository;

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(logger);

// AppSettings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddHttpContextAccessor();

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey)),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorization();

// EF Core
builder.Services.AddDbContext<NorthwindContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Northwind"));
});

// DI
builder.Services.AddScoped<IUnitOfWork, NorthwindContext>();
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddScoped<IDistributedLock>(sp =>
{
    var uow = sp.GetRequiredService<IUnitOfWork>();
    return new SqlDistributedLock(uow);
});
builder.Services.AddScoped<Dispatcher>();
builder.Services.AddScoped(typeof(ICrudService<,>), typeof(CrudService<,>));

// Đăng ký ICacheService và BusinessCacheService
builder.Services.AddScoped<ICacheService, DistributedCacheService>();
builder.Services.AddScoped<BusinessCacheService>();

builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(_ => { }, typeof(AutoMapperProfile));
builder.Services.AddHandlers(typeof(Dispatcher).Assembly);

// Đăng ký SQL Server Cache
builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("Northwind");
    options.SchemaName = "dbo";
    options.TableName = "CacheTable";
});

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
app.UseMiddleware<ApiLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();