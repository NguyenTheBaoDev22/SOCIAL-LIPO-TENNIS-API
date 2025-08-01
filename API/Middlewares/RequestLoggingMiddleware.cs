namespace API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            var traceId = context.TraceIdentifier;
            _logger.LogInformation("Request {Method} {Path} | TraceId: {TraceId} | Body: {Body}",
                context.Request.Method, context.Request.Path, traceId, body);

            await _next(context);
        }
    }
}
