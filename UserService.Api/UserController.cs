using Microsoft.AspNetCore.Mvc;
using UserService.Application;
using UserService.Core.Entities;

namespace UserService.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAllUsers());

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var user = _service.GetUser(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            user.Id = Guid.NewGuid();
            _service.CreateUser(user);
            return Created($"/api/users/{user.Id}", user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] User user)
        {
            user.Id = id;
            _service.UpdateUser(user);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _service.DeleteUser(id);
            return NoContent();
        }
    }
}
