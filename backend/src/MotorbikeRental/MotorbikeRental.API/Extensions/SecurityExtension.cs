using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using MotorbikeRental.Domain.Entities.User;
using Microsoft.IdentityModel.Tokens;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.API.Extensions
{
    public static class SecurityExtension
    {
        public static IServiceCollection RegisterSecurityService(this IServiceCollection services, IConfiguration configuration)
        {
            #region Identity Configuration
            services.AddIdentity<UserCredentials, Roles>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<MotorbikeRentalDbContext>()
            .AddDefaultTokenProviders();
            #endregion
            #region JWT configuration
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                byte[] key = Encoding.UTF8.GetBytes(configuration["AppSettings:JwtConfig:Secret"]);
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["AppSettings:JwtConfig:ValidAudience"],
                    ValidIssuer = configuration["AppSettings:JwtConfig:ValidIssuer"],
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion
            return services;
        }
    }
}