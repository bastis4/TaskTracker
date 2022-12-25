using FluentValidation;
using TaskTracker.Models.Project.Requests;

namespace TaskTracker.Models.Project.Validators
{
    public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
    {
        public CreateProjectRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("The length of {PropertyName} must be 100 characters or fewer");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(y => y.StartDate)
                .WithMessage("EndDate must be after StartDate");

            RuleFor(x => x.Status)
                .IsInEnum();

            RuleFor(x => x.Priority)
                .IsInEnum();
        }
    }
}
