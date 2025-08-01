using FluentValidation;
using MediatR;
using Serilog;
using Shared.Results;
using System.Diagnostics;

namespace Applications.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request,
                                        RequestHandlerDelegate<TResponse> next,
                                        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count == 0)
        {
            return await next();
        }

        var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();
        var requestType = typeof(TRequest).Name;
        var errorMessages = failures.Select(f => f.ErrorMessage).ToList();

        Log.Warning("Validation failed | TraceId: {TraceId} | Request: {RequestType} | Errors: {@Errors}",
            traceId, requestType, errorMessages);

        // Nếu là BaseResponse<T>, trả về lỗi chuẩn hóa
        if (typeof(TResponse).IsGenericType &&
            typeof(TResponse).GetGenericTypeDefinition() == typeof(BaseResponse<>))
        {
            var dataType = typeof(TResponse).GenericTypeArguments[0];
            var errorMessage = string.Join(" | ", errorMessages);

            var method = typeof(BaseResponse<>)
                .MakeGenericType(dataType)
                .GetMethod(nameof(BaseResponse<object>.Error), new[] { typeof(string), typeof(string), typeof(string) });

            var errorResponse = method?.Invoke(null, new object[] { errorMessage, "VALIDATION_FAILED", traceId });

            return errorResponse as TResponse
                   ?? throw new InvalidOperationException("Unable to create BaseResponse<T> error.");
        }

        // Nếu không phải BaseResponse => throw để bubble lên
        throw new ValidationException(string.Join(" | ", errorMessages));
    }
}
