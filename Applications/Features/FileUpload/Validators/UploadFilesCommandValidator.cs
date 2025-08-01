using Applications.Features.FileUpload.Commands;
using FluentValidation;

namespace Applications.Features.FileUpload.Validators
{
    public class UploadFilesCommandValidator : AbstractValidator<UploadMultipleFilesCommand>
    {
        public UploadFilesCommandValidator()
        {
            RuleForEach(x => x.Files).SetValidator(new SingleFileValidator());
        }
    }

}
