using Application.Configuration.Commands;
using Application.Configuration.Queries;
using Application.UserAccess.CommandHandlers;
using Application.UserAccess.Commands;
using Application.UserAccess.Queries;
using Application.UserAccess.QueryHandlers;
using LabDesk.BuildingBlocks.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. CẤU HÌNH CONTROLLERS & API EXPLORER
// ==========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ==========================================
// 2. CẤU HÌNH CORS
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ==========================================
// 3. CẤU HÌNH JWT AUTHENTICATION & AUTHORIZATION
// ==========================================
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtSettings = jwtSection.Get<JwtSettings>();

if (jwtSettings != null)
{
    var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
}
builder.Services.AddAuthorization(); // Bổ sung đăng ký dịch vụ phân quyền

// ==========================================
// 4. ĐĂNG KÝ DEPENDENCY INJECTION (DI)
// ==========================================
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();

// Command & Query Handlers
builder.Services.AddScoped<ICommandHandler<RegisterUserCommand>, RegisterUserCommandHandler>();
builder.Services.AddScoped<IQueryHandler<LoginUserQuery, LoginResultDto>, LoginUserQueryHandler>();

// ==========================================
// 5. CẤU HÌNH SWAGGER
// ==========================================
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LabDesk API",
        Version = "v1",
        Description = "Hệ thống API LabDesk - Modular Monolith"
    });
});

var app = builder.Build();

// ==========================================
// 6. CẤU HÌNH HTTP REQUEST PIPELINE (MIDDLEWARE)
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Thứ tự bắt buộc: Authentication phải đứng trước Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();