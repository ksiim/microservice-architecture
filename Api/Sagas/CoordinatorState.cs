using System;
using MassTransit;
using Automatonymous;

namespace Api.Sagas
{
    public class CoordinatorState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public string ProjectId { get; set; }
        public DateTime Created { get; set; }
    }
}
