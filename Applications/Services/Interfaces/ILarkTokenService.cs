using Applications.Features.LarksuiteIntegrations.DTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services.Interfaces
{
    public interface ILarkTokenService
    {
        Task<LarkTokens?> GetAccessTokenAsync(string code, string state);
        Task<LarkTokens?> RefreshAccessTokenAsync(string refreshToken);
        Task<LarkTokenCache?> GetValidTokenAsync();
    }
}
