using Applications.Features.ImportData.Queries;
using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Results.Extensions;
namespace Applications.Features.ImportData.Handlers
{
    public class GetAllAdministrativeUnitsQueryPaginatedHandler : IRequestHandler<GetAllAdministrativeUnitsPaginatedQuery, BaseResponse<PaginatedResult<ProvinceWithCommunesDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAdministrativeUnitsQueryPaginatedHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<PaginatedResult<ProvinceWithCommunesDto>>> Handle(GetAllAdministrativeUnitsPaginatedQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.ProvinceRepositories.AsQueryable()
                .Where(p => !p.IsDeleted)
                .Include(p => p.Communes)
                .AsQueryable(); // 👈 Ép kiểu lại để gọi được extension

            var result = await query.ToPaginatedListAsync<Province, ProvinceWithCommunesDto>(
                _mapper,
                page: request.Page,
                pageSize: request.PageSize,
                cancellationToken: cancellationToken
            );

            return BaseResponse<PaginatedResult<ProvinceWithCommunesDto>>.Success(result);

        }

    }
}
