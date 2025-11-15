using System;
using MassTransit;
using Automatonymous;

namespace Api.Sagas
{
    public class OrchestratorState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public string ProjectId { get; set; }
        public DateTime StartedAt { get; set; }
    }
}
