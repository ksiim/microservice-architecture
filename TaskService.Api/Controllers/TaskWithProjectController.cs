using Microsoft.AspNetCore.Mvc;
using TaskService.Logic;

namespace TaskService.Api.Controllers
{
    [ApiController]
    [Route("tasks-with-project")] // пример join
    public class TaskWithProjectController : ControllerBase
    {
        private readonly TaskService _service;
        public TaskWithProjectController(TaskService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTasksWithProject()
        {
            var result = _service.GetTasksWithProject();
            return Ok(result);
        }
    }
}