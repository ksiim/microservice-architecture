using Microsoft.AspNetCore.Mvc;
using CoreLib.Models;
using TaskService.Logic;

namespace TaskService.Api.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _service;
        public TaskController(TaskService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Task task)
        {
            _service.CreateTask(task);
            return Created($"/tasks/{task.Id}", task);
        }

        [HttpGet("{taskId}")]
        public IActionResult GetById(string taskId)
        {
            var task = _service.GetTask(taskId);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] string project_id)
        {
            var tasks = _service.GetTasks(project_id);
            return Ok(tasks);
        }

        [HttpPatch("{taskId}")]
        public IActionResult Patch(string taskId, [FromBody] Task patch)
        {
            var task = _service.GetTask(taskId);
            if (task == null) return NotFound();
            if (patch.Status != 0) task.Status = patch.Status;
            if (patch.Priority != 0) task.Priority = patch.Priority;
            if (!string.IsNullOrEmpty(patch.Description)) task.Description = patch.Description;
            if (!string.IsNullOrEmpty(patch.AssigneeId)) task.AssigneeId = patch.AssigneeId;
            _service.UpdateTask(task);
            return Ok(task);
        }

        [HttpDelete("{taskId}")]
        public IActionResult Delete(string taskId)
        {
            _service.DeleteTask(taskId);
            return NoContent();
        }

        [HttpPost("{taskId}/assign")]
        public IActionResult Assign(string taskId, [FromBody] AssignRequest req)
        {
            _service.AssignTask(taskId, req.AssigneeId);
            return Ok();
        }
    }

    public class AssignRequest
    {
        public string AssigneeId { get; set; }
    }
}