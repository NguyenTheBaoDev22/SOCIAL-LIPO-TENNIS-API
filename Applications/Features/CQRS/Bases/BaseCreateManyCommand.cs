using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.CQRS.Bases
{
    /// <summary>
    /// Command CQRS cho việc tạo nhiều DTO cùng lúc.
    /// </summary>
    /// <typeparam name="TDto">Kiểu DTO</typeparam>
    public class BaseCreateManyCommand<TDto> : IRequest<BaseResponse<IEnumerable<TDto>>>
    {
        public IEnumerable<TDto> Data { get; }

        public BaseCreateManyCommand(IEnumerable<TDto> data)
        {
            Data = data;
        }
    }
}
