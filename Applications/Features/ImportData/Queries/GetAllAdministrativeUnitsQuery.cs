using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.ImportData.Queries
{

    public class GetAllAdministrativeUnitsPaginatedQuery : IRequest<BaseResponse<PaginatedResult<ProvinceWithCommunesDto>>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 4000;
    }
    public class GetAllAdministrativeUnitsQuery : IRequest<BaseResponse<List<ProvinceWithCommunesDto>>>
    {
    }
}
