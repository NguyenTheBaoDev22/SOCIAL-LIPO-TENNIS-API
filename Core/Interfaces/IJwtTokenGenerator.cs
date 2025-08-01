using Core.Entities;
using Shared.DTOs;
using System.Security.Claims;

namespace Core.Interfaces
{
    ///// <summary>
    ///// Interface định nghĩa phương thức tạo JWT token từ entity User.
    ///// </summary>
    //public interface IJwtTokenGenerator
    //{
    //    /// <summary>
    //    /// Tạo JWT token cho user.
    //    /// </summary>
    //    /// <param name="user">Entity User</param>
    //    /// <returns>Token string</returns>
    //    TokenResult GenerateToken(ClientCredential client);
    //    string GenerateRefreshToken();  // Phương thức tạo refresh token
    //}

    public interface IJwtTokenGenerator
    { 
        
        string GenerateToken(IEnumerable<Claim> claims);
    }
}
