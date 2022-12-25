global using AutoMapper;
using TaskTracker.DAL.Models;
using TaskTracker.Models.Project.Requests;
using TaskTracker.Models.Task.Requests;
using TaskTracker.Models.Project.Responses;
using TaskTracker.Models.Task.Responses;
using TaskTracker.Models.Project;

namespace TaskTracker
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region "Project mapping"
            CreateMap<CreateProjectRequest, ProjectEntity>();
            CreateMap<ProjectEntity, CreateProjectResponse>();
            CreateMap<ProjectEntity, Project>().ReverseMap();

            CreateMap<GetProjectRequest, ProjectEntity>();
            CreateMap<ProjectEntity, GetProjectResponse>().ForMember(dest => dest.Tasks, opt => opt.MapFrom(source => source.Tasks));

            CreateMap<UpdateProjectRequest, ProjectEntity>();
            CreateMap<ProjectEntity, UpdateProjectResponse>();

            CreateMap<DeleteProjectRequest, ProjectEntity>();
            CreateMap<List<ProjectEntity>, GetProjectsResponse>().ForMember(dest => dest.Projects, opt => opt.MapFrom(source => source));
            #endregion

            #region "Task mapping"
            CreateMap<CreateTaskRequest, TaskEntity>();
            CreateMap<TaskEntity, CreateTaskResponse>();

            CreateMap<GetTaskRequest, TaskEntity>();
            CreateMap<TaskEntity, GetTaskResponse>();

            CreateMap<UpdateTaskRequest, TaskEntity>();
            CreateMap<TaskEntity, UpdateTaskResponse>();

            CreateMap<DeleteTaskRequest, TaskEntity>();
            CreateMap<Models.Task.Task, TaskEntity>().ReverseMap();
            #endregion
        }
    }
}
