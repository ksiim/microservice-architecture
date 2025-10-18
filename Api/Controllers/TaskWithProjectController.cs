using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CoreLib.Models;
using TaskService.Logic;

namespace Api.Controllers
{
    [ApiController]
    [Route("tasks-with-project")]
    public class TaskWithProjectController : ControllerBase
    {
        private readonly TaskService.Logic.TaskService _service;
        public TaskWithProjectController(TaskService.Logic.TaskService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTasksWithProject()
        {
            var result = _service.GetTasksWithProject()
                .Select(tp => new TaskWithProjectDto
                {
                    TaskId = tp.Item1.Id,
                    TaskTitle = tp.Item1.Title,
                    ProjectId = tp.Item2.Id,
                    ProjectName = tp.Item2.Name
                });
            return Ok(result);
        }

        public class TaskWithProjectDto
        {
            public string TaskId { get; set; }
            public string TaskTitle { get; set; }
            public string ProjectId { get; set; }
            public string ProjectName { get; set; }
        }
    }
}
