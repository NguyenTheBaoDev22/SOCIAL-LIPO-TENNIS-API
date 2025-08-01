using Applications.Features.ZaloIntegrations.ZaloAuth.DTOs;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services.Interfaces
{
    public interface IZaloAuthenticationService
    {
        Task<BaseResponse<UserZaloIdentityResponseModel>> GetUserInfoFromZaloAsync(string accessToken, string zaloToken);
    }

}
