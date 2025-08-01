// Infrastructure/Identity/JwtTokenGenerator.cs
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Identity
{
    //public class JwtTokenGenerator : IJwtTokenGenerator
    //{
    //    private readonly IConfiguration _configuration;

    //    public JwtTokenGenerator(IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //    }

    //    public string GenerateToken(List<Claim> customClaims)
    //    {
    //        var secretKey = _configuration["JwtSettings:Service:Secret"]!;
    //        var issuer = _configuration["JwtSettings:Service:Issuer"]!;
    //        var audience = _configuration["JwtSettings:Service:Audience"]!;
    //        var expiryMinutes = int.Parse(_configuration["JwtSettings:Service:ExpiryInMinutes"] ?? "10080");

    //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    //        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //        var claims = new List<Claim>
    //        {
    //            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    //        };
    //        claims.AddRange(customClaims);

    //        var token = new JwtSecurityToken(
    //            issuer,
    //            audience,
    //            claims,
    //            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
    //            signingCredentials: credentials
    //        );

    //        return new JwtSecurityTokenHandler().WriteToken(token);
    //    }
    //}
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtServiceOptions _options;

        public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _options = jwtSettings.Value.Service;
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_options.ExpiryInMinutes);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
