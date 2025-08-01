using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.CQRS.Bases
{
    public class BaseDeleteCommand : IRequest<BaseResponse<object>>
    {
        public Guid Id { get; set; }
    }
}
