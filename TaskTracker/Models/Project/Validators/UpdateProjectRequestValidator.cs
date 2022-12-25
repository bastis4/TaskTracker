using FluentValidation;
using TaskTracker.Models.Project.Requests;
using TaskTracker.DAL.Interfaces;

namespace TaskTracker.Models.Project.Validators
{
    public class UpdateProjectRequestValidator : AbstractValidator<UpdateProjectRequest>
    {
        private readonly IProjectRepository _projectRepository;

        public UpdateProjectRequestValidator(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;

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

            RuleFor(x => x.Id)
                .MustAsync(ProjectExists)
                .WithMessage("Project with Id {PropertyValue} doesn't exist");
        }

        public async Task<bool> ProjectExists(int projectId, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetRecord(projectId, cancellationToken);

            return project != null;
        }
    }
}
