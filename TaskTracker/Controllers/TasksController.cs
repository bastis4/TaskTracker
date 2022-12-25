using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.AspNetCore;
using TaskTracker.Models.Task.Requests;
using TaskTracker.Interfaces;
using TaskTracker.Models.Project.Requests;
using TaskTracker.Models.Project.Validators;
using TaskTracker.Services;
using TaskTracker.DAL.Interfaces;
using TaskTracker.Models.Task.Validators;

namespace TaskTracker.Controllers
{
    public class TasksController : BaseApiController
    {
        private ITaskService _taskService;
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;

        public TasksController(ITaskService taskService, ITaskRepository taskRepository, IProjectRepository projectRepository)
        {
            _taskService = taskService;
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
        }

        /// <summary>
        /// Create a task
        /// </summary>
        /// <param name="request">Request to create a task without ID</param>
        /// <returns>Response containig task created with ID</returns>
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            var validator = new CreateTaskRequestValidator(_projectRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);
                return BadRequest(validationResult);
            }

            var response = await _taskService.CreateTask(request);
            
            return Ok(response);
        }

        /// <summary>
        /// Get a task by ID
        /// </summary>
        /// <param name="request">Request with task ID</param>
        /// <returns>Response containing found task</returns>
        [HttpGet]
        public async Task<IActionResult> GetTask([FromQuery] GetTaskRequest request)
        {
            var validator = new GetTaskRequestValidator(_taskRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(validationResult);
            }

            var response = await _taskService.GetTask(request);

            return Ok(response);
        }

        /// <summary>
        /// Update a task by ID
        /// </summary>
        /// <param name="request">Request with ID of a task and properties needed to be change</param>
        /// <returns>Response with updated task</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateTaskRequest request)
        {
            var validator = new UpdateTaskRequestValidator(_taskRepository, _projectRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(validationResult);
            }

            var response = await _taskService.UpdateProject(request);

            return Ok(response);
        }

        /// <summary>
        /// Delete a task by ID
        /// </summary>
        /// <param name="id">ID of a task to be deleted</param>
        /// <returns>Web response</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var validator = new DeleteTaskRequestValidator(_taskRepository);
            var validationResult = await validator.ValidateAsync(id);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(validationResult);
            }

            await _taskService.DeleteTask(id);

            return Ok();
        }
    }
}
