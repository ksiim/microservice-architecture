using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using CoreLib.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("projects")]
    public class ProjectController : ControllerBase
    {
        private readonly TaskService.Logic.ProjectService _service;
        public ProjectController(TaskService.Logic.ProjectService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Project project)
        {
            _service.CreateProject(project);
            return Created($"/projects/{project.Id}", project);
        }

        [HttpGet("{projectId}")]
        public IActionResult GetById(string projectId)
        {
            var project = _service.GetProject(projectId);
            if (project == null) return NotFound();
            return Ok(project);
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var projects = _service.GetProjects();
            return Ok(projects);
        }

        [HttpPatch("{projectId}")]
        public IActionResult Patch(string projectId, [FromBody] Project patch)
        {
            var project = _service.GetProject(projectId);
            if (project == null) return NotFound();
            if (!string.IsNullOrEmpty(patch.Name)) project.Name = patch.Name;
            _service.UpdateProject(project);
            return Ok(project);
        }

        [HttpDelete("{projectId}")]
        public IActionResult Delete(string projectId)
        {
            _service.DeleteProject(projectId);
            return NoContent();
        }
    }
}
