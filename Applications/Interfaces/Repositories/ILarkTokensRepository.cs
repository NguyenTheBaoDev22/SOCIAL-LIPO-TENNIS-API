using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interfaces.Repositories
{
    public interface ILarkTokensRepository : IBaseRepository<LarkTokens>
    {
        // Thêm method custom nếu cần, ví dụ:
        Task<LarkTokens?> GetActiveTokenByUserAsync(Guid userId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Lấy token còn hạn gần nhất (AccessToken chưa hết hạn, sort theo CreatedAt mới nhất)
        /// </summary>
        Task<LarkTokens?> GetLatestValidAsync(CancellationToken cancellationToken = default);
    }
}
