namespace Infrastructure.Services
{
    //public class TokenService : ITokenService
    //{
    //    private readonly IConfiguration _configuration;

    //    public int AccessTokenExpirationMinutes { get; private set; }

    //    public TokenService(IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //        AccessTokenExpirationMinutes = 60; // hoặc lấy từ config
    //    }

    //    public string GenerateAccessToken(User user)
    //    {
    //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //        var claims = new[]
    //        {
    //            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
    //            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
    //            new Claim(JwtRegisteredClaimNames.Email, user.Email),
    //            // Thêm claim theo nhu cầu
    //        };

    //        var token = new JwtSecurityToken(
    //            issuer: _configuration["Jwt:Issuer"],
    //            audience: _configuration["Jwt:Audience"],
    //            claims: claims,
    //            expires: DateTime.UtcNow.AddMinutes(AccessTokenExpirationMinutes),
    //            signingCredentials: creds
    //        );

    //        return new JwtSecurityTokenHandler().WriteToken(token);
    //    }

    //    public (string Token, DateTime ExpiresAt) GenerateRefreshToken()
    //    {
    //        var randomBytes = new byte[32];
    //        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
    //        rng.GetBytes(randomBytes);

    //        var token = Convert.ToBase64String(randomBytes);
    //        var expiresAt = DateTime.UtcNow.AddDays(7);

    //        return (token, expiresAt);
    //    }
    //}
}
