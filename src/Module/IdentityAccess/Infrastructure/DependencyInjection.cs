using Application.Interface;
using Application.Interface.Security;
using BuildingBlocks.Application;
using Infrastructure.Authentication;
using Infrastructure.Authentication.AccessToken;
using Infrastructure.Authentication.RefreshToken;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityAccessInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<IdentityAccessDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddOptions<JwtSettings>()
                .Bind(configuration.GetSection(JwtSettings.SectionName))
                .Validate(x => !string.IsNullOrWhiteSpace(x.Issuer), "Jwt:Issuer is required.")
                .Validate(x => !string.IsNullOrWhiteSpace(x.Audience), "Jwt:Audience is required.")
                .Validate(x => System.Text.Encoding.UTF8.GetByteCount(x.SecretKey) >= 32, "Jwt:SecretKey must be at least 32 UTF-8 bytes.")
                .Validate(x => x.ExpirationInMinutes is >= 1 and <= 60, "Jwt:ExpirationInMinutes must be between 1 and 60.")
                .ValidateOnStart();
            services.AddOptions<RefreshTokenSettings>()
                .Bind(configuration.GetSection(RefreshTokenSettings.SectionName))
                .Validate(x => x.ExpirationInDays is >= 1 and <= 30, "RefreshToken:ExpirationInDays must be between 1 and 30.")
                .Validate(x => x.AbsoluteExpirationInDays >= x.ExpirationInDays && x.AbsoluteExpirationInDays <= 90, "RefreshToken:AbsoluteExpirationInDays must be between expiration days and 90.")
                .Validate(x => x.AbsoluteExpirationInDays <= 30, "RefreshToken:AbsoluteExpirationInDays must be no more than 30 days.")
                .ValidateOnStart();

            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddSingleton<IRefreshTokenService, RefreshTokenService>();
            services.AddSingleton<IAccessTokenLifetime, AccessTokenLifetime>();
            services.AddSingleton<IRefreshTokenLifetime, RefreshTokenLifetime>();
            services.AddScoped<Verify,VerifyHasher>();
            services.AddScoped<ITokenProvider,JwtTokenProvider>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUnitOfWorkIdentityAccess, IdentityAccessUnitOfWork>();

            return services;
        }
    }
}
