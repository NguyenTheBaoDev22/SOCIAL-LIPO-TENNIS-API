using Core.Entities.Integrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.Repositories
{
    public interface ILarkEmailLogRepository : IBaseRepository<LarkEmailLog>
    {
        // Nếu có method đặc thù riêng cho LarkEmailLog, định nghĩa ở đây
    }
}
