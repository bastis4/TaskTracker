using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Controllers;
using TaskTracker.DAL.Interfaces;
using TaskTracker.DAL.Repositories;
using TaskTracker.Interfaces;
using TaskTracker.Models.Project.Enums;
using TaskTracker.Models.Project.Requests;
using TaskTracker.Models.Project.Responses;
using TaskTracker.Services;
using TaskTracker.Models.Task.Requests;
using TaskTracker.Models.Task.Responses;

namespace TaskTracker.Tests
{
    public class TestTasksController
    {
        private IProjectRepository _projectRepository;
        private ITaskRepository _taskRepository;
        private IWrapperRepository _wrapperRepository;

        private ITaskService _taskService;
        private IProjectService _projectService;

        private TaskTrackerDbContext _context;
        private TasksController _taskController;
        private ProjectsController _projectsController;

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TaskTrackerDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new TaskTrackerDbContext(options);
            _taskRepository = new TaskRepository(_context);
            _projectRepository = new ProjectRepository(_context);
            _wrapperRepository = new WrapperRepository(_context);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();

            _taskService = new TaskService(_wrapperRepository, mapper);
            _projectService = new ProjectService(_wrapperRepository, mapper);

            _taskController = new TasksController(_taskService, _taskRepository, _projectRepository);
            _projectsController = new ProjectsController(_projectService, _projectRepository);
        }

        [OneTimeTearDown]
        public void Clean()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task CreateTask_ShouldCreateTask()
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

            response = await _taskController.CreateTask(new CreateTaskRequest()
            {
                ProjectId = createProjectResponse.Id,
                Name = "Задача #1",
                Description = "Описание важной задачи",
                Priority = Models.Task.Enums.TaskPriority.High,
                Status = Models.Task.Enums.TaskStatus.ToDo
            });

            var createTaskResponse = (response as OkObjectResult).Value as CreateTaskResponse;

            Assert.NotZero(createTaskResponse.Id);
        }

        [Test]
        public async Task GetTask_ShouldFailOnGetNonExistTask()
        {
            var response = await _taskController.GetTask(new GetTaskRequest
            {
                Id = int.MaxValue
            });

            Assert.True(response is BadRequestObjectResult);
        }

        [Test]
        public async Task Update_ShouldUpdateTask()
        {
            var response = await _projectsController.CreateProject(new CreateProjectRequest
            {
                Name = $"Проект 1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                Priority = ProjectPriority.Epic,
                Status = ProjectStatus.Active
            });

            var createProjectResponse = (response as OkObjectResult).Value as CreateProjectResponse;

            Assert.NotZero(createProjectResponse.Id);

            var task = new CreateTaskRequest
            {
                ProjectId = createProjectResponse.Id,
                Name = "Задача #1",
                Description = "Описание важной задачи",
                Priority = Models.Task.Enums.TaskPriority.High,
                Status = Models.Task.Enums.TaskStatus.ToDo
            };

            response = await _taskController.CreateTask(task);

            var createTaskResponse = (response as OkObjectResult).Value as CreateTaskResponse;

            Assert.NotZero(createTaskResponse.Id);

            var updateProjectRequest = new UpdateTaskRequest
            {
                Id = createTaskResponse.Id,
                ProjectId = createProjectResponse.Id,
                Name = "Задача #777",
                Description = "Описание взятой в работу задачи",
                Priority = Models.Task.Enums.TaskPriority.Low,
                Status = Models.Task.Enums.TaskStatus.InProgress
            };

            response = await _taskController.Update(updateProjectRequest);

            var updateTaskResponse = (response as OkObjectResult) .Value as UpdateTaskResponse;

            Assert.That(createTaskResponse.Id, Is.EqualTo(updateTaskResponse.Id));
            Assert.That(task.Name, Is.Not.EqualTo(updateTaskResponse.Name));
            Assert.That(task.Description, Is.Not.EqualTo(updateTaskResponse.Description));
            Assert.That(task.Priority, Is.Not.EqualTo(updateTaskResponse.Priority));
            Assert.That(task.Status, Is.Not.EqualTo(updateTaskResponse.Status));
        }

        [Test]
        public async Task Delete_ShouldDeleteTask()
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

            response = await _taskController.CreateTask(new CreateTaskRequest()
            {
                ProjectId = createProjectResponse.Id,
                Name = "Задача #1",
                Description = "Описание важной задачи",
                Priority = Models.Task.Enums.TaskPriority.High,
                Status = Models.Task.Enums.TaskStatus.ToDo
            });

            var createTaskResponse = (response as OkObjectResult).Value as CreateTaskResponse;

            Assert.NotZero(createTaskResponse.Id);

            response = await _taskController.Delete(createTaskResponse.Id);

            Assert.True(response is OkResult);

            response = await _taskController.GetTask(new GetTaskRequest
            {
                Id = createTaskResponse.Id
            });

            Assert.True(response is BadRequestObjectResult);
        }
    }
}