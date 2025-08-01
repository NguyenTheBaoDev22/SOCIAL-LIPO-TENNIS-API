using MediatR;

namespace Applications.Commons
{
    public abstract class BaseQuery<TResponse> : IRequest<TResponse>
    {
        public Guid CorrelationId { get; set; } = Guid.NewGuid();
        public DateTime RequestTime { get; set; } = DateTime.UtcNow;
    }
}
