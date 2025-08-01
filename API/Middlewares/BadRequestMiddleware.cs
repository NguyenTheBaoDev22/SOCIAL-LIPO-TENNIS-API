using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Middlewares
{
    public class BadRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public BadRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBody = context.Response.Body;

            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            if (context.Response.StatusCode == 400 &&
                context.Response.ContentType?.Contains("application/problem+json") == true)
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                var body = await new StreamReader(memoryStream).ReadToEndAsync();
                var traceId = context.TraceIdentifier;

                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var customError = new
                {
                    code = "VALIDATION_FAILED",
                    message = "Dữ liệu không hợp lệ.",
                    errors = problemDetails?.Extensions["errors"],
                    traceId
                };

                context.Response.Body = originalBody;
                context.Response.ContentType = "application/json";
                memoryStream.SetLength(0);
                await context.Response.WriteAsJsonAsync(customError);
            }
            else
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBody);
                context.Response.Body = originalBody;
            }
        }
    }

}
