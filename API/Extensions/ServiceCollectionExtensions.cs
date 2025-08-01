using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var secret = configuration["JwtSettings:AppUser:Secret"];
            var issuer = configuration["JwtSettings:AppUser:Issuer"];
            var audience = configuration["JwtSettings:AppUser:Audience"];

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!))
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
