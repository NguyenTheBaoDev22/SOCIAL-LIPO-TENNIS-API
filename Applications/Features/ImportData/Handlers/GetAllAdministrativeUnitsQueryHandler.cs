using Applications.Features.ImportData.Queries;
using Applications.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.ImportData.Handlers
{
    public class GetAllAdministrativeUnitsQueryHandler : IRequestHandler<GetAllAdministrativeUnitsQuery, BaseResponse<List<ProvinceWithCommunesDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAdministrativeUnitsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<ProvinceWithCommunesDto>>> Handle(GetAllAdministrativeUnitsQuery request, CancellationToken cancellationToken)
        {
            var provinces = await _unitOfWork.ProvinceRepositories.AsQueryable()
                .Where(p => !p.IsDeleted)
                .Include(p => p.Communes)
                .ToListAsync(cancellationToken);

            var result = _mapper.Map<List<ProvinceWithCommunesDto>>(provinces);

            return BaseResponse<List<ProvinceWithCommunesDto>>.Success(result);
        }
    }

}
