using TaskTracker.Models.Project.Requests;
using TaskTracker.Models.Project.Responses;

namespace TaskTracker.Interfaces
{
    public interface IProjectService
    {
        Task<CreateProjectResponse> CreateProject(CreateProjectRequest request, CancellationToken cancellationToken = default);
        Task<GetProjectResponse> GetProject(GetProjectRequest request, CancellationToken cancellationToken = default);
        Task<UpdateProjectResponse> UpdateProject(UpdateProjectRequest request, CancellationToken cancellationToken = default);
        Task DeleteProject(int id, CancellationToken cancellationToken = default);
        Task<GetProjectsResponse> GetProjects(GetProjectsRequest request);
    }
}
