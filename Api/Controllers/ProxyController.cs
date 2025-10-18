using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Http;
using TaskModel = CoreLib.Models.Task;
using ProjectModel = CoreLib.Models.Project;
using TaskService.Logic;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/proxy")]
    public class ProxyController : ControllerBase
    {
        private readonly IHttpService _http;
        private readonly TaskService.Logic.TaskService _taskService;
        private readonly TaskService.Logic.ProjectService _projectService;

        public ProxyController(IHttpService http, TaskService.Logic.TaskService taskService, TaskService.Logic.ProjectService projectService)
        {
            _http = http;
            _taskService = taskService;
            _projectService = projectService;
        }

        // Service B (Task): returns task by id
        [HttpGet("task/{taskId}")]
        public IActionResult GetTask(string taskId)
        {
            var task = _taskService.GetTask(taskId);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // Service A (Project): returns project + attached tasks by calling Service B via IHttpService
        [HttpGet("project/{projectId}/attached")]
        public async Task<IActionResult> GetProjectWithAttachedTasks(string projectId)
        {
            var project = _projectService.GetProject(projectId);
            if (project == null) return NotFound();

            // call service B (task list filtered by project_id)
            var url = $"{Request.Scheme}://{Request.Host}/api/tasks?project_id={projectId}";

            var tasks = await _http.GetAsync<List<TaskModel>>(url);

            return Ok(new
            {
                Project = project,
                AttachedTasks = tasks ?? new List<TaskModel>()
            });
        }
    }
}
