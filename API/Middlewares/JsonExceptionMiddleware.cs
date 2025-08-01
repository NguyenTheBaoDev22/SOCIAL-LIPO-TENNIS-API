using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Results;
using System.Text.Json;

namespace API.Middlewares
{
    public class JsonExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JsonExceptionMiddleware> _logger;

        public JsonExceptionMiddleware(RequestDelegate next, ILogger<JsonExceptionMiddleware> logger)
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
            catch (JsonException ex)
            {
                await HandleJsonExceptionAsync(context, ex, "JSON_PARSE_ERROR", "Dữ liệu JSON không hợp lệ.");
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("reading the request body"))
            {
                await HandleJsonExceptionAsync(context, ex, "REQUEST_BODY_INVALID", "Không thể đọc dữ liệu từ body.");
            }
        }

        private async Task HandleJsonExceptionAsync(HttpContext context, Exception ex, string code, string userMessage)
        {
            var traceId = context.TraceIdentifier;

            _logger.LogError(ex, "🚨 JSON request error | TraceId: {TraceId} | Path: {Path}", traceId, context.Request.Path);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = BaseResponse<object>.Error(code, userMessage, traceId);
            response.Errors = new Dictionary<string, string[]>
            {
                { "RequestBody", new[] { ex.Message } }
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
