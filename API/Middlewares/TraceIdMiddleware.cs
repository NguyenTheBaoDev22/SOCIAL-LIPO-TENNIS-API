using Shared.Interfaces;

namespace API.Middlewares
{
    //public class TraceIdMiddleware
    //{
    //    private readonly RequestDelegate _next;

    //    public TraceIdMiddleware(RequestDelegate next)
    //    {
    //        _next = next;
    //    }

    //    public async Task Invoke(HttpContext context)
    //    {
    //        // Nếu chưa có Activity, tạo mới Activity cho TraceId
    //        if (Activity.Current == null)
    //        {
    //            var activity = new Activity("IncomingRequest");
    //            activity.Start();
    //        }

    //        // Tiếp tục xử lý request
    //        await _next(context);
    //    }
    //}
    public class TraceIdMiddleware
    {
        private const string TraceIdHeader = "X-Trace-Id";
        private readonly RequestDelegate _next;
        private readonly ILogger<TraceIdMiddleware> _logger;

        public TraceIdMiddleware(RequestDelegate next, ILogger<TraceIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, ICurrentUserService currentUser)
        {
            string traceId = context.Request.Headers.TryGetValue(TraceIdHeader, out var values)
                ? values.FirstOrDefault() ?? Guid.NewGuid().ToString()
                : Guid.NewGuid().ToString();

            context.Items["TraceId"] = traceId;
            currentUser.SetTraceId(traceId);

            using (_logger.BeginScope(new Dictionary<string, object> { ["TraceId"] = traceId }))
            {
                await _next(context);
            }
        }
    }

}
