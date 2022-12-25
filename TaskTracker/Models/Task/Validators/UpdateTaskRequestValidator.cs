using FluentValidation;
using TaskTracker.DAL.Interfaces;
using TaskTracker.DAL.Repositories;
using TaskTracker.Models.Task.Requests;

namespace TaskTracker.Models.Task.Validators
{
    public class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
    {
        private readonly ITaskRepository _taskRepository;
        private IProjectRepository _projectRepository;

        public UpdateTaskRequestValidator(ITaskRepository taskRepository, IProjectRepository projectRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;

            RuleFor(x => x.Id)
                .MustAsync(TaskExists)
                .WithMessage("Task with Id {PropertyValue} doesn't exist");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("The length of {PropertyName} must be 100 characters or fewer");

            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .MustAsync(ProjectExists)
                .WithMessage("Project with Id {PropertyValue} doesn't exist");


            RuleFor(x => x.Status)
                .IsInEnum();

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(300)
                .WithMessage("The length of {PropertyName} must be 300 characters or fewer");

            RuleFor(x => x.Priority)
                .IsInEnum();
        }

        public async Task<bool> TaskExists(int taskId, CancellationToken cancellationToken)
        {
            var project = await _taskRepository.GetRecord(taskId, cancellationToken);

            return project != null;
        }

        public async Task<bool> ProjectExists(int projectId, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetRecord(projectId, cancellationToken);
            return project != null;
        }
    }
}

