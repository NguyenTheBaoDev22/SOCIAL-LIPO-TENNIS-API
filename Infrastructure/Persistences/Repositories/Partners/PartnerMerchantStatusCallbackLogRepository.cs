using Applications.Interfaces.Repositories.Partners;
using AutoMapper;
using Core.Entities.Partners;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistences.Repositories.Partners
{
    public class PartnerMerchantStatusCallbackLogRepository
    : BaseRepository<PartnerMerchantStatusCallbackLog>, IPartnerMerchantStatusCallbackLogRepository
    {
        private readonly AppDbContext _context;

        public PartnerMerchantStatusCallbackLogRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<PartnerMerchantStatusCallbackLog>> logger)
            : base(context, mapper, currentUser, logger)
        {
            _context = context;
        }
    }
}
