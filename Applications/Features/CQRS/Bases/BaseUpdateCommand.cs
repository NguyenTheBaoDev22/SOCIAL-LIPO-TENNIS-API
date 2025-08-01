using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.CQRS.Bases
{
    public class BaseUpdateCommand<TDto> : IRequest<BaseResponse<TDto>>
    {
        public Guid Id { get; set; }
        public TDto Data { get; set; }

        public BaseUpdateCommand(Guid id, TDto data)
        {
            Id = id;
            Data = data;
        }
    }
}
