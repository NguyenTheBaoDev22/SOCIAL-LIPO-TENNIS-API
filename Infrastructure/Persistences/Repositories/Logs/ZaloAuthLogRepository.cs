using Applications.Interfaces.Repositories.Logs;
using AutoMapper;
using Core.Entities.Logs;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistences.Repositories.Logs
{
    public class ZaloAuthLogRepository : BaseRepository<ZaloAuthLog>, IZaloAuthLogRepository
    {
        public ZaloAuthLogRepository(
            AppDbContext dbContext,
            IMapper mapper,
            ICurrentUserService currentUser, // 👈 thêm dòng này
            ILogger<BaseRepository<ZaloAuthLog>> logger)
            : base(dbContext, mapper, currentUser, logger) // 👈 truyền vào đây luôn
        {
        }
    }
}
