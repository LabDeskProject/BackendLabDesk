using IdentityAccess.API;
using Application.Interface;
using System.Security.Claims;
using OrganizationManagement.API;
using ServiceCatalog.API;
using ServiceCatalog.Infrastructure;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using RequestManagement.Infrastructure;
using RequestManagement.API;
using WorkflowApproval.API;
using WorkflowApproval.Infrastructure;
using Assignment.API;
using Assignment.Infrastructure;
using SLA.API;
using SLA.Infrastructure;
using Audit.API;
using Audit.Infrastructure;
using Notification.API;
using Notification.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(IdentityAccessApiMarker).Assembly)
    .AddApplicationPart(typeof(OrganizationManagement.API.Marker).Assembly)
    .AddApplicationPart(typeof(ServiceCatalog.API.Marker).Assembly)
    .AddApplicationPart(typeof(RequestManagement.API.Marker).Assembly)
    .AddApplicationPart(typeof(WorkflowApproval.API.Marker).Assembly)
    .AddApplicationPart(typeof(Assignment.API.Marker).Assembly)
    .AddApplicationPart(typeof(SLA.API.Marker).Assembly)
    .AddApplicationPart(typeof(Audit.API.Marker).Assembly)
    .AddApplicationPart(typeof(Notification.API.Marker).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập JWT Access Token vào đây."
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

builder.Services.AddIdentityAccessModule(builder.Configuration);
builder.Services.AddOrganizationManagementModule(builder.Configuration);
builder.Services.AddServiceCatalogModule(builder.Configuration);
builder.Services.AddRequestManagementModule(builder.Configuration);
builder.Services.AddWorkflowApprovalModule(builder.Configuration);
builder.Services.AddAssignmentModule(builder.Configuration);
builder.Services.AddSlaModule(builder.Configuration);
builder.Services.AddAuditModule(builder.Configuration);
builder.Services.AddNotificationModule(builder.Configuration);
builder.Services.AddHttpContextAccessor();

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
    ?? throw new InvalidOperationException("JWT settings are not configured.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.Name,
            ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };
        options.MapInboundClaims = true;
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var userIdValue = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!Guid.TryParse(userIdValue, out var userId)) { context.Fail("Invalid user identity."); return; }
                await using var scope = context.HttpContext.RequestServices.CreateAsyncScope();
                var users = scope.ServiceProvider.GetRequiredService<ILoginRepository>();
                var user = await users.GetByIdAsync(userId, context.HttpContext.RequestAborted);
                if (user is null || !user.IsActive) { context.Fail("User is inactive."); return; }
                if (context.Principal?.IsInRole("Admin") == true && !user.IsGlobalAdmin) context.Fail("Global administrator privilege was revoked.");
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();
app.Use(async (context, next) =>
{
    try { await next(); }
    catch (Exception ex) when (!context.Response.HasStarted)
    {
        var (status, title) = ex switch
        {
            UnauthorizedAccessException => (StatusCodes.Status403Forbidden, "Forbidden"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
            InvalidOperationException => (StatusCodes.Status409Conflict, "Conflict"),
            FluentValidation.ValidationException => (StatusCodes.Status400BadRequest, "Validation Failed"),
            ArgumentException => (StatusCodes.Status400BadRequest, "Invalid Request"),
            _ => (StatusCodes.Status500InternalServerError, "Server Error")
        };
        context.Response.Clear();
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";
        var detail = status == StatusCodes.Status500InternalServerError ? "An unexpected error occurred." : ex.Message;
        await context.Response.WriteAsJsonAsync(new { type = "about:blank", title, status, detail, traceId = context.TraceIdentifier });
    }
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
