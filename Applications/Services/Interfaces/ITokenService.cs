using Core.Entities.AppUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateResetPasswordToken(Guid userId);
        bool ValidateResetPasswordToken(string token, out Guid userId);
    }

}
