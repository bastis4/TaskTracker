using TaskTracker.DAL.Interfaces;
using TaskEntity = TaskTracker.DAL.Models.TaskEntity;
using TaskTracker.Models.Task.Requests;
using TaskTracker.Models.Task.Responses;
using TaskTracker.Interfaces;

namespace TaskTracker.Services
{
    public class TaskService : ITaskService
    {
        private readonly IWrapperRepository _repository;
        private readonly IMapper _mapper;
        public TaskService(IWrapperRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CreateTaskResponse> CreateTask(CreateTaskRequest request, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<TaskEntity>(request);
            var entity = await _repository.Task.AddRecord(model, cancellationToken);
            await _repository.SaveAsync(cancellationToken);
            var response = _mapper.Map<CreateTaskResponse>(entity);

            return response;
        }

        public async Task<GetTaskResponse> GetTask(GetTaskRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.Task.GetRecord(request.Id, cancellationToken);

            var response = _mapper.Map<GetTaskResponse>(entity);

            return response;
        }

        public async Task<UpdateTaskResponse> UpdateProject(UpdateTaskRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.Task.GetRecord(request.Id, cancellationToken);
            _mapper.Map(request, entity);

            await _repository.SaveAsync(cancellationToken);

            var response = _mapper.Map<UpdateTaskResponse>(entity);
            return response;
        }

        public async Task DeleteTask(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.Task.GetRecord(id, cancellationToken);

            _repository.Task.DeleteRecord(entity);
            await _repository.SaveAsync(cancellationToken);
        }
    }
}
