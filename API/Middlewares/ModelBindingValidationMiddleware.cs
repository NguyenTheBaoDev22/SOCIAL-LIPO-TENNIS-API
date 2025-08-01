using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Text.Json;

public class ModelBindingValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ModelBindingValidationMiddleware> _logger;

    public ModelBindingValidationMiddleware(RequestDelegate next, ILogger<ModelBindingValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var method = context.Request.Method;
        if (method == HttpMethods.Post || method == HttpMethods.Put || method == HttpMethods.Patch)
        {
            var endpoint = context.GetEndpoint();
            var actionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();

            if (actionDescriptor != null)
            {
                var bodyParam = actionDescriptor.Parameters
                    .FirstOrDefault(p => p.BindingInfo?.BindingSource == BindingSource.Body);

                if (bodyParam != null)
                {
                    context.Request.EnableBuffering();
                    using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                    var body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;

                    var traceId = context.TraceIdentifier;

                    // 1. Body rỗng
                    if (string.IsNullOrWhiteSpace(body))
                    {
                        await ReturnError(context, traceId, bodyParam.Name!, "Request body is required.");
                        return;
                    }

                    try
                    {
                        var jsonDoc = JsonDocument.Parse(body);

                        // 2. Body là null
                        if (jsonDoc.RootElement.ValueKind == JsonValueKind.Null)
                        {
                            await ReturnError(context, traceId, bodyParam.Name!, "Request body cannot be null.");
                            return;
                        }

                        // 3. Body không phải object
                        if (jsonDoc.RootElement.ValueKind != JsonValueKind.Object)
                        {
                            await ReturnError(context, traceId, bodyParam.Name!, "Request body must be a JSON object.");
                            return;
                        }
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogWarning(ex, "Invalid JSON format in request body | TraceId: {TraceId}", traceId);
                        await ReturnError(context, traceId, bodyParam.Name!, "Malformed JSON. Cannot parse request body.");
                        return;
                    }
                }
            }
        }

        await _next(context);
    }

    private async Task ReturnError(HttpContext context, string traceId, string paramName, string errorMessage)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var response = new
        {
            code = "400",
            message = "Request body validation failed.",
            errors = new Dictionary<string, string[]>
            {
                { paramName, new[] { errorMessage } }
            },
            traceId,
            timestamp = DateTime.UtcNow,
            isSuccess = false
        };

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}
