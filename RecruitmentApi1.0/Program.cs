using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecruitmentApi1._0.Data;
using RecruitmentApi1._0.Services;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ================= SERILOG =================
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// ================= SERVICES =================

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=recruitment.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

// 🔹 Scrutor auto-DI
builder.Services.Scan(scan => scan
    .FromAssemblyOf<ITestService>()
    .AddClasses(c => c.Where(t => t.Name.EndsWith("Service")))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// HostedService
builder.Services.AddHostedService<HealthCheckHostedService>();

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("SUPER_SECRET_KEY_12345"))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ================= MIDDLEWARE =================

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/minimal/ping", () => "Minimal API працює");

app.Run();
