using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    [HttpGet("")]
    public IActionResult GetTasks()
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("{id:guid}")]
    public IActionResult GetTask(Guid id)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("")]
    public IActionResult CreateTask()
    {
        throw new NotImplementedException();
    }
    
    [HttpPut("{id:guid}")]
    public IActionResult UpdateTask(Guid id)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteTask(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// добавить исполнителя к задаче
    /// </summary>
    [HttpPut("{id}/assign")]
    public IActionResult AssignTask(Guid id, [FromBody] List<Guid> userIDs)
    {
        throw new NotImplementedException();
    }
}