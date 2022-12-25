using FluentValidation;
using TaskTracker.Models.Task.Requests;
using TaskTracker.DAL.Interfaces;

namespace TaskTracker.Models.Task.Validators
{
    public class GetTaskRequestValidator : AbstractValidator<GetTaskRequest>
    {
        private ITaskRepository _taskRepository;
        public GetTaskRequestValidator(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;

            RuleFor(x => x.Id)
                .MustAsync(TaskExists)
                .WithMessage("Task with Id {PropertyValue} doesn't exist");

        }

        public async Task<bool> TaskExists(int taskId, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetRecord(taskId, cancellationToken);

            return task != null;
        }
    }
}
