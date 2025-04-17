//  Application/Features/Resumes/Commands/ParseResumeCommandValidator.cs

using FluentValidation;

namespace Application.Features.Resumes.Commands.Validators
{
    public class ParseResumeCommandValidator : AbstractValidator<ParseResumeCommand>
    {
        public ParseResumeCommandValidator()
        {
            RuleFor(x => x.FileBytes)
                .NotEmpty().WithMessage("File content is required.");

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("File name is required.")
                .Must(f => f.EndsWith(".pdf") || f.EndsWith(".docx"))
                .WithMessage("Only .pdf and .docx file types are supported.");
        }
    }
}
