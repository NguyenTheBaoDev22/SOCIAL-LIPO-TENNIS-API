using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Filters
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Kiểm tra tham số có kiểu IFormFile không
            var formFileParameters = context.ApiDescription.ParameterDescriptions
                .Where(p => p.ParameterDescriptor.ParameterType == typeof(IFormFile))
                .ToList();

            if (formFileParameters.Any())
            {
                // Nếu có tham số IFormFile, thêm thông tin vào Swagger UI
                foreach (var parameter in formFileParameters)
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = parameter.Name,
                        In = ParameterLocation.Query,  // Dùng 'Query' cho file upload
                        Description = "File to upload",
                        Required = true,
                        Schema = new OpenApiSchema { Type = "string", Format = "binary" }  // Định dạng tệp là binary
                    });
                }
            }
        }
    }
}
