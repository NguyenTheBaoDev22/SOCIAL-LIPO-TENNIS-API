using Applications.Features.FileUpload.Commands;
using FluentValidation;

namespace Applications.Features.FileUpload.Validators
{
    public class UploadMultipleFilesCommandValidator : AbstractValidator<UploadMultipleFilesCommand>
    {
        public UploadMultipleFilesCommandValidator()
        {
            RuleForEach(x => x.Files).SetValidator(new SingleFileValidator());
        }
    }

}
