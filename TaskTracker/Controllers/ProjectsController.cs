using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.AspNetCore;
using TaskTracker.DAL.Interfaces;
using TaskTracker.Models.Project.Requests;
using TaskTracker.Models.Project.Responses;
using TaskTracker.Interfaces;
using TaskTracker.Models.Project.Validators;

namespace TaskTracker.Controllers
{
    public class ProjectsController : BaseApiController
    {
        private IProjectService _projectService;
        private readonly IProjectRepository _projectRepository;

        public ProjectsController(IProjectService projectService, IProjectRepository projectRepository)
        {
            _projectService = projectService;
            _projectRepository = projectRepository;
        }

        /// <summary>
        /// Create a new project
        /// </summary>
        /// <param name="request">Request to create a new project</param>
        /// <returns>Response containing created project with ID</returns>
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
        {
            var validator = new CreateProjectRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(validationResult);
            }
            
            var response = await _projectService.CreateProject(request);
            
            return Ok(response);
        }

        /// <summary>
        /// Get project by ID
        /// </summary>
        /// <param name="request">Request with ID</param>
        /// <returns>Response containg project with all tasks</returns>
        [HttpGet]
        public async Task<IActionResult> GetProject([FromQuery] GetProjectRequest request)
        {
            var validator = new GetProjectRequestValidator(_projectRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(validationResult);
            }
            
            var response = await _projectService.GetProject(request);

            return Ok(response);
        }

        /// <summary>
        /// Get projects by fields using sorting and raging
        /// </summary>
        /// <param name="request">Request with search properties</param>
        /// <returns>Response with found projects</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetProjects([FromQuery] GetProjectsRequest request)
        {
            var response = await _projectService.GetProjects(request);

            return Ok(response);
        }

        /// <summary>
        /// Update a project by ID
        /// </summary>
        /// <param name="request">Request with needed properties to update and ID</param>
        /// <returns>Response with an updated project</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateProjectRequest request)
        {
            var validator = new UpdateProjectRequestValidator(_projectRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(validationResult);
            }

            var response = await _projectService.UpdateProject(request);

            return Ok(response);
        }

        /// <summary>
        /// Delete a project by ID
        /// </summary>
        /// <param name="id">ID of a project to be deleted</param>
        /// <returns>Web response</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var validator = new DeleteProjectRequestValidator(_projectRepository);
            var validationResult = await validator.ValidateAsync(id);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(validationResult);
            }

            await _projectService.DeleteProject(id);

            return Ok();
        }
    }
}
