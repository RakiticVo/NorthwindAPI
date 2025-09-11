using Serilog;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NorthwindApi.Application;
using NorthwindApi.Application.Abstractions;
using NorthwindApi.Application.Common;
using NorthwindApi.Application.Common.DateTimes;
using NorthwindApi.Application.Mapping;
using NorthwindApi.Infrastructure.Cache;
using NorthwindApi.Infrastructure.Security;
using NorthwindApi.Infrastructure.Middlewares;
using NorthwindApi.Persistence;
using NorthwindApi.Persistence.Locking;
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
        // Custom Unauthorized (chưa đăng nhập / token invalid)
        o.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse(); // Ngăn ASP.NET Core trả về mặc định
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync(
                    "{\n" +
                    "\"statusCode\": 401,\n" +
                    "\"message\": \"Please login!!!\"" +
                    "\n}");
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync(
                    "{\n" +
                    "\"statusCode\": 403,\n" +
                    "\"message\": \"You aren't Admin!!!\"" +
                    "\n}");
            }
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireClaim("userRole", "1");
    });

// EF Core
builder.Services.AddDbContext<NorthwindContext>(opt =>
{
    opt.UseSqlServer(
        builder.Configuration.GetConnectionString("Northwind"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        )
    );
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Northwind API", Version = "v1" });

    // Thêm JWT Bearer vào Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập JWT theo format: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});
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

// Middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind API V1");
    c.RoutePrefix = string.Empty; // <-- quan trọng, để / ra swagger
});
app.UseMiddleware<ApiLoggingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();