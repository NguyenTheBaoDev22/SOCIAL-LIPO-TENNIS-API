namespace Shared.Exceptions
{
    public class BusinessException : Exception
    {
        public string Code { get; }
        public string? TraceId { get; }

        public BusinessException(string message, string code, string? traceId = null)
            : base(message)
        {
            Code = code;
            TraceId = traceId;
        }

        public BusinessException(string message, string code, Exception innerException, string? traceId = null)
            : base(message, innerException)
        {
            Code = code;
            TraceId = traceId;
        }
    }
}
