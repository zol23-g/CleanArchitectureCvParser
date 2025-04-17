//  Application/Features/Resumes/Queries/Validators/GetResumeByIdQueryValidator.cs

using FluentValidation;

namespace Application.Features.Resumes.Queries.Validators
{
    public class GetResumeByIdQueryValidator : AbstractValidator<GetResumeByIdQuery>
    {
        public GetResumeByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Resume ID must be greater than zero.");
        }
    }
}
