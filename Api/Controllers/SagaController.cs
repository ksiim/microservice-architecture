using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/saga")]
    public class SagaController : ControllerBase
    {
        private readonly IPublishEndpoint _publisher;

        public SagaController(IPublishEndpoint publisher)
        {
            _publisher = publisher;
        }

        [HttpPost("coordinator/start/{projectId}")]
        public async Task<IActionResult> StartCoordinator(string projectId)
        {
            var correlationId = Guid.NewGuid();
            await _publisher.Publish(new Api.Sagas.StartCoordinator(correlationId, projectId));
            return Accepted(new { correlationId, projectId });
        }

        [HttpPost("orchestrator/start/{projectId}")]
        public async Task<IActionResult> StartOrchestrator(string projectId)
        {
            var correlationId = Guid.NewGuid();
            await _publisher.Publish(new Api.Sagas.OrchestratorStart(correlationId, projectId));
            return Accepted(new { correlationId, projectId });
        }
    }
}
