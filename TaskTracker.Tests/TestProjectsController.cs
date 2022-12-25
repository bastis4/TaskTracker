using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.DAL.Interfaces;
using TaskTracker.DAL.Repositories;
using TaskTracker.Interfaces;
using TaskTracker.Models.Project.Enums;
using TaskTracker.Models.Project.Requests;
using TaskTracker.Models.Project.Responses;
using TaskTracker.Services;

namespace TaskTracker.Tests
{
    public class TestProjectsController
    {
        private IProjectRepository _projectRepository;
        private IWrapperRepository _wrapperRepository;

        private IProjectService _projectService;

        private TaskTrackerDbContext _context;
        private ProjectsController _projectsController;

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TaskTrackerDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new TaskTrackerDbContext(options);
            _projectRepository = new ProjectRepository(_context);
            _wrapperRepository = new WrapperRepository(_context);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();

            _projectService = new ProjectService(_wrapperRepository, mapper);
            _projectsController = new ProjectsController(_projectService, _projectRepository);
        }

        [OneTimeTearDown]
        public void Clean()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task CreateProject_ShouldCreateProject()
        {
            var project = new CreateProjectRequest
            {
                Name = $"Проект 1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                Priority = ProjectPriority.Epic,
                Status = ProjectStatus.Active
            };

            var response = await _projectsController.CreateProject(project);
            
            var createProjectResponse = (response as OkObjectResult).Value as CreateProjectResponse;

            Assert.NotZero(createProjectResponse.Id);

            response = await _projectsController.GetProject(new Models.Project.Requests.GetProjectRequest
            {
                Id = createProjectResponse.Id
            });

            var getProjectResponse = (response as OkObjectResult).Value as GetProjectResponse;

            Assert.That(createProjectResponse.Id, Is.EqualTo(getProjectResponse.Id));
            Assert.That(project.Name, Is.EqualTo(getProjectResponse.Name));
            Assert.That(project.StartDate, Is.EqualTo(getProjectResponse.StartDate));
            Assert.That(project.EndDate, Is.EqualTo(getProjectResponse.EndDate));
            Assert.That(project.Priority, Is.EqualTo(getProjectResponse.Priority));
            Assert.That(project.Status, Is.EqualTo(getProjectResponse.Status));
        }

        [Test]
        public async Task GetProject_ShouldFailOnGetNonExistProject()
        {
            var response = await _projectsController.GetProject(new Models.Project.Requests.GetProjectRequest
            {
                Id = int.MaxValue
            });

            Assert.True(response is BadRequestObjectResult);
        }

        [Test]
        public async Task Update_ShouldUpdateProject()
        {
            var project = new Models.Project.Requests.CreateProjectRequest
            {
                Name = $"Проект 1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                Priority = ProjectPriority.Epic,
                Status = ProjectStatus.Active
            };

            var response = await _projectsController.CreateProject(project);

            var createProjectResponse = (response as OkObjectResult).Value as CreateProjectResponse;

            Assert.NotZero(createProjectResponse.Id);

            var updateProjectRequest = new UpdateProjectRequest
            {
                Id = createProjectResponse.Id,
                Name = $"Проект 777",
                StartDate = DateTime.Now.AddDays(30),
                EndDate = DateTime.Now.AddDays(365),
                Priority = ProjectPriority.Medium,
                Status = ProjectStatus.Completed
            };

            response = await _projectsController.Update(updateProjectRequest);

            var updateProjectResponse = (UpdateProjectResponse)(response as OkObjectResult).Value;

            Assert.That(createProjectResponse.Id, Is.EqualTo(updateProjectResponse.Id));
            Assert.That(project.Name, Is.Not.EqualTo(updateProjectResponse.Name));
            Assert.That(project.StartDate, Is.Not.EqualTo(updateProjectResponse.StartDate));
            Assert.That(project.EndDate, Is.Not.EqualTo(updateProjectResponse.EndDate));
            Assert.That(project.Priority, Is.Not.EqualTo(updateProjectResponse.Priority));
            Assert.That(project.Status, Is.Not.EqualTo(updateProjectResponse.Status));
        }

        [Test]
        public async Task Delete_ShouldDeleteProject()
        {
            var project = new CreateProjectRequest
            {
                Name = $"Проект 1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                Priority = ProjectPriority.Epic,
                Status = ProjectStatus.Active
            };

            var response = await _projectsController.CreateProject(project);

            var createProjectResponse = (response as OkObjectResult).Value as CreateProjectResponse;

            Assert.NotZero(createProjectResponse.Id);

            response = await _projectsController.Delete(createProjectResponse.Id);

            Assert.True(response is Microsoft.AspNetCore.Mvc.OkResult);

            response = await _projectsController.GetProject(new GetProjectRequest
            {
                Id = createProjectResponse.Id
            });

            Assert.True(response is BadRequestObjectResult);
        }


        [Test]
        public async Task GetProjects_ShouldReturnProjectsByFilter()
        {
            //Clean Projects table
            _context.Projects.RemoveRange(_context.Projects);
            await _context.SaveChangesAsync();

            //Create some projects in DB
            var random = new Random();
            var newProjects = new List<CreateProjectRequest>();

            for (int i = 1; i <= 10; i++)
            {
                var project = new CreateProjectRequest
                {
                    Name = $"Проект {i}",
                    StartDate = DateTime.Now.AddDays(-random.Next(365)),
                    EndDate = DateTime.Now.AddDays(random.Next(365)),
                    Priority = (ProjectPriority) random.Next(0, 3),
                    Status = (ProjectStatus) random.Next(0, 3)
                };

                var createProjectResponse = await _projectsController.CreateProject(project);
                Assert.NotZero(((createProjectResponse as OkObjectResult).Value as CreateProjectResponse).Id);

                newProjects.Add(project);
            }

            //Testing SearchName filter
            var searchNameFilter = "1";
            var response = await _projectsController.GetProjects(new GetProjectsRequest

            {
                SearchName = searchNameFilter
            });

            var filteredProjects = ((response as OkObjectResult).Value as GetProjectsResponse).Projects;
            Assert.That(filteredProjects.Count, Is.EqualTo(newProjects.Count(x => x.Name.Contains(searchNameFilter))));


            //Testing StartDate filter
            var startDateFrom = DateTime.Now.AddDays(-100);
            var startDateTo = DateTime.Now;

            response = await _projectsController.GetProjects(new GetProjectsRequest

            {
                StartDate = new Models.Common.Classes.Range<DateTime?> { From = startDateFrom, To = startDateTo }
            });

            filteredProjects = ((response as OkObjectResult).Value as GetProjectsResponse).Projects;
            Assert.That(filteredProjects.Count, Is.EqualTo(newProjects.Count(x => x.StartDate >= startDateFrom && x.StartDate <= startDateTo)));


            //Testing EndDate filter
            var endDateFrom = DateTime.Now;
            var endDateTo = DateTime.Now.AddDays(100); 

            response = await _projectsController.GetProjects(new GetProjectsRequest

            {
                EndDate = new Models.Common.Classes.Range<DateTime?> { From = endDateFrom, To = endDateTo }
            });

            filteredProjects = ((response as OkObjectResult).Value as GetProjectsResponse).Projects;
            Assert.That(filteredProjects.Count, Is.EqualTo(newProjects.Count(x => x.EndDate >= endDateFrom && x.EndDate <= endDateTo)));


            //Testing Priorities filter
            var priorities = new List<ProjectPriority>() { ProjectPriority.Medium, ProjectPriority.Epic };

            response = await _projectsController.GetProjects(new GetProjectsRequest

            {
                Priorities = priorities
            });

            filteredProjects = ((response as OkObjectResult).Value as GetProjectsResponse).Projects;
            Assert.That(filteredProjects.Count, Is.EqualTo(newProjects.Count(x => priorities.Contains(x.Priority))));


            //Testing Statuses filter
            var statuses = new List<ProjectStatus>() { ProjectStatus.Active, ProjectStatus.Completed};

            response = await _projectsController.GetProjects(new GetProjectsRequest

            {
                Statuses = statuses
            });

            filteredProjects = ((response as OkObjectResult).Value as GetProjectsResponse).Projects;
            Assert.That(filteredProjects.Count, Is.EqualTo(newProjects.Count(x => statuses.Contains(x.Status))));
        }
    }
}