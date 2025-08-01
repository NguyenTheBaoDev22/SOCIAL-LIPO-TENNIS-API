using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.CQRS.Bases
{
    public class BaseCreateCommand<TDto> : IRequest<BaseResponse<TDto>>
    {
        public TDto Data { get; set; }

        public BaseCreateCommand(TDto data)
        {
            Data = data;
        }
    }
}
