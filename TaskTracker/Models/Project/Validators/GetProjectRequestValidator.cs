using FluentValidation;
using TaskTracker.Models.Project.Requests;
using TaskTracker.DAL.Interfaces;

namespace TaskTracker.Models.Project.Validators
{
    public class GetProjectRequestValidator : AbstractValidator<GetProjectRequest>
    {
        private IProjectRepository _projectRepository;
        public GetProjectRequestValidator(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;

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
