using TaskTracker.DAL.Interfaces;
using TaskTracker.DAL.Models;
using TaskTracker.Models.Project.Requests;
using TaskTracker.Models.Project.Responses;
using TaskTracker.Interfaces;
using TaskTracker.Models.Project.Enums;
using TaskTracker.Models.Common.Enums;

namespace TaskTracker.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IWrapperRepository _repository;
        private readonly IMapper _mapper;
        public ProjectService(IWrapperRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CreateProjectResponse> CreateProject(CreateProjectRequest request, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<ProjectEntity>(request);

            await _repository.Project.AddRecord(entity, cancellationToken);
            await _repository.SaveAsync(cancellationToken);

            var response = _mapper.Map<CreateProjectResponse>(entity);

            return response;
        }

        public async Task<GetProjectResponse> GetProject(GetProjectRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.Project.GetProjectWithAllTasks(request.Id);

            var response = _mapper.Map<GetProjectResponse>(entity);

            return response;
        }

        public async Task<UpdateProjectResponse> UpdateProject(UpdateProjectRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.Project.GetRecord(request.Id, cancellationToken);
            _mapper.Map(request, entity);

            await _repository.SaveAsync(cancellationToken);

            var response = _mapper.Map<UpdateProjectResponse>(entity);
            return response;
        }

        public async Task DeleteProject(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.Project.GetRecord(id, cancellationToken);

            _repository.Project.DeleteRecord(entity);
            await _repository.SaveAsync(cancellationToken);
        }

        public async Task<GetProjectsResponse> GetProjects(GetProjectsRequest request)
        {
            IQueryable<ProjectEntity> query = _repository.Project.Table;

            if(!string.IsNullOrWhiteSpace(request.SearchName))
            {
                query = query.Where(x => x.Name.Contains(request.SearchName));
            }

            if(request.StartDate?.From != null)
            {
                query = query.Where(x => x.StartDate >= request.StartDate.From);
            }

            if (request.StartDate?.To != null)
            {
                query = query.Where(x => x.StartDate <= request.StartDate.To);
            }

            if (request.EndDate?.From != null)
            {
                query = query.Where(x => x.EndDate >= request.EndDate.From);
            }

            if (request.EndDate?.To != null)
            {
                query = query.Where(x => x.EndDate <= request.EndDate.To);
            }

            var statuses = request.Statuses?.Select(x => (int)x).ToArray();

            if (statuses?.Length > 0)
            {
                query = query.Where(x => statuses.Contains(x.Status));
            }

            var priorities = request.Priorities?.Select(x => (int)x).ToArray();

            if (priorities?.Length > 0)
            {
                query = query.Where(x => priorities.Contains(x.Priority));
            }

            query = request.SortType switch
            {
                ProjectSortType.ByName => request.SortOrder == SortOrder.Descending 
                ? query.OrderByDescending(x => x.Name)
                : query.OrderBy(x => x.Name),

                ProjectSortType.ByStartDate => request.SortOrder == SortOrder.Descending  
                ? query.OrderByDescending(x => x.StartDate)
                : query.OrderBy(x => x.StartDate),

                ProjectSortType.ByEndDate => request.SortOrder == SortOrder.Descending
                ? query.OrderByDescending(x => x.StartDate)
                : query.OrderBy(x => x.EndDate),

                ProjectSortType.ByStatus => request.SortOrder == SortOrder.Descending
                ? query.OrderByDescending(x => x.Status)
                : query.OrderBy(x => x.Status),

                ProjectSortType.ByPriority => request.SortOrder == SortOrder.Descending
                ? query.OrderByDescending(x => x.Priority)
                : query.OrderBy(x => x.Priority),
                _ => query,
            };

            var entities = query.ToList();

            var response = _mapper.Map<GetProjectsResponse>(entities);
            return response;
        }
    }
}
