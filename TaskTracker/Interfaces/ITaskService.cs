using TaskTracker.Models.Task.Requests;
using TaskTracker.Models.Task.Responses;

namespace TaskTracker.Interfaces
{
    public interface ITaskService
    {
        Task<CreateTaskResponse> CreateTask(CreateTaskRequest request, CancellationToken cancellationToken = default);
        Task<GetTaskResponse> GetTask(GetTaskRequest request, CancellationToken cancellationToken = default);
        Task<UpdateTaskResponse> UpdateProject(UpdateTaskRequest request, CancellationToken cancellationToken = default);
        Task DeleteTask(int id, CancellationToken cancellationToken = default);
    }
}
