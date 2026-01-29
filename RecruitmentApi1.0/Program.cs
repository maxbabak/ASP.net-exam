using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecruitmentApi1._0.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =======================
// SERVICES
// =======================

// Controllers
builder.Services.AddControllers();

// DbContext + SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=recruitment.db"));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 CACHING
builder.Services.AddMemoryCache();

// Authentication (JWT)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("SUPER_SECRET_KEY_12345"))
        };
    });

// Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// =======================
// MIDDLEWARE
// =======================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// =======================
// ENDPOINTS
// =======================

app.MapControllers();

// Minimal API
app.MapGet("/minimal/ping", () => "Minimal API працює");

app.Run();