using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Results.Extensions
{
    public static class ErrorCodeMapper
    {
        public static int ToHttpStatusCode(string errorCode)
        {
            if (string.IsNullOrWhiteSpace(errorCode)) return 500;

            return errorCode[..2] switch
            {
                "01" => 400, // Merchant logic lỗi đầu vào
                "02" => 402, // Payment lỗi → Payment Required
                "03" => 401, // Auth → Unauthorized
                "04" => 409, // Order → Conflict
                "05" => 422, // Resource → Unprocessable Entity
                "06" => 403, // Authorization → Forbidden
                "07" => 503, // API/Service → Service Unavailable
                "08" => 422, // Notification
                "09" => 440, // Session expired (custom code)
                "99" => 500, // System error
                _ => 500
            };
        }
    }

}
