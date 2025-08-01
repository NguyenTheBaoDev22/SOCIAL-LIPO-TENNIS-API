using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities.Integrations;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistences.Repositories
{
    public class LarkEmailLogRepository : BaseRepository<LarkEmailLog>, ILarkEmailLogRepository
    {
        public LarkEmailLogRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<LarkEmailLog>> logger)
            : base(context, mapper, currentUser, logger)
        {
        }

        // Add custom methods here if needed
    }
}
