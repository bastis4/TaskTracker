using FluentValidation;
using TaskTracker.DAL.Interfaces;

namespace TaskTracker.Models.Task.Validators
{
    public class DeleteTaskRequestValidator : AbstractValidator<int>
    {
        private ITaskRepository _taskRepository;
        public DeleteTaskRequestValidator(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;

            RuleFor(taskId => taskId)
                .MustAsync(TaskExists)
                .WithMessage("Task with Id {PropertyValue} doesn't exist");
        }

        public async Task<bool> TaskExists(int taskId, CancellationToken cancellationToken)
        {
            var project = await _taskRepository.GetRecord(taskId, cancellationToken);

            return project != null;
        }
    }
}

