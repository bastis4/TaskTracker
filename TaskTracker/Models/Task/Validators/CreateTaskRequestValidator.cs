using FluentValidation;
using System.Runtime.CompilerServices;
using TaskTracker.Models.Task.Requests;
using TaskTracker.DAL.Interfaces;
using TaskTracker.Interfaces;
using TaskTracker.DAL.Repositories;
using TaskTracker.Models.Project;

namespace TaskTracker.Models.Task.Validators
{
    public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
    {
        private IProjectRepository _projectRepository;

        public CreateTaskRequestValidator(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;

            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .MustAsync(ProjectExists)
                .WithMessage("Project with Id {PropertyValue} doesn't exist");


            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("The length of {PropertyName} must be 100 characters or fewer");

            RuleFor(x => x.Status)
                .IsInEnum();

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(300)
                .WithMessage("The length of {PropertyName} must be 300 characters or fewer");

            RuleFor(x => x.Priority)
                .IsInEnum();
        }

        public async Task<bool> ProjectExists(int projectId, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetRecord(projectId, cancellationToken);
            return project != null;
        }
    }
}
