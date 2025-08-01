using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Applications.Features.FileUpload.Validators
{
    public class SingleFileValidator : AbstractValidator<IFormFile>
    {
        public SingleFileValidator()
        {
            RuleFor(x => x.Length)
                .LessThanOrEqualTo(20 * 1024 * 1024)
                .WithMessage("Dung lượng tối đa mỗi file là 20MB.");

            //RuleFor(x => x.ContentType)
            //    .Must(type => type.StartsWith("image/") ||
            //                  type == "application/pdf" ||
            //                  type == "video/mp4")
            //    .WithMessage("Chỉ hỗ trợ ảnh, PDF và video MP4.");
        }
    }
}
