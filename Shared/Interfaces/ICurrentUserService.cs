using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? PhoneNumber { get; }
        string? Email { get; }
        Guid? TenantId { get; }
        Guid? MerchantId { get; }
        Guid? MerchantBranchId { get; }

        List<string> Roles { get; }
        List<string> Permissions { get; }

        bool IsAuthenticated { get; }

        string? TraceId { get; }
        void SetTraceId(string traceId);

        string? IpAddress { get; }
        string? UserAgent { get; }
    }
}
