using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Results;

namespace Shared.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
                    );

                var response = BaseResponse<object>.Error("Dữ liệu không hợp lệ", "400");
                response.Errors = errors;
                response.TraceId = context.HttpContext.TraceIdentifier;

                context.Result = new JsonResult(response)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }
    }
}
