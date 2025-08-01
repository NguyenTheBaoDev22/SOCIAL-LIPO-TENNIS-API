using Core.Entities.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.Repositories.Logs
{
    public interface IZaloAuthLogRepository : IBaseRepository<ZaloAuthLog>
    {
        // Add custom queries specific to ZaloAuthLog if needed in future
    }
}
