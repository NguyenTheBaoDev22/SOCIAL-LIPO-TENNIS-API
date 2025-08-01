using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface ISpamProtectionService
    {
        Task<bool> IsSpamAsync(string key, TimeSpan window, CancellationToken cancellationToken = default);
        Task MarkAsync(string key, TimeSpan window, CancellationToken cancellationToken = default);
    }


}
