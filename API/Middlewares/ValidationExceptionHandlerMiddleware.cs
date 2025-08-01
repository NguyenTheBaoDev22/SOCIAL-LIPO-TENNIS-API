using FluentValidation;

namespace API.Middlewares
{
    public class ValidationExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationExceptionHandlerMiddleware> _logger;

        public ValidationExceptionHandlerMiddleware(RequestDelegate next, ILogger<ValidationExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                var traceId = context.TraceIdentifier;
                var errors = ex.Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                _logger.LogWarning("FluentValidation error | TraceId: {TraceId} | Errors: {@Errors}", traceId, errors);

                var response = new
                {
                    code = "VALIDATION_FAILED",
                    message = "Dữ liệu không hợp lệ.",
                    errors,
                    traceId
                };

                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

}
