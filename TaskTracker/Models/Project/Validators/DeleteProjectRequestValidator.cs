using FluentValidation;
using TaskTracker.DAL.Interfaces;

namespace TaskTracker.Models.Project.Validators
{
    public class DeleteProjectRequestValidator : AbstractValidator<int>
    {
        private IProjectRepository _projectRepository;
        public DeleteProjectRequestValidator(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;

            RuleFor(projectId => projectId)
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
