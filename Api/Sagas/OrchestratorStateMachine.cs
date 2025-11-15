using System;
using MassTransit;
using MassTransit.Saga;
using Automatonymous;

namespace Api.Sagas
{
    public record ProjectCreatedEvent(Guid CorrelationId, string ProjectId);
    public record OrchestratorStart(Guid CorrelationId, string ProjectId);

    public class OrchestratorStateMachine : MassTransitStateMachine<OrchestratorState>
    {
        public State WaitingForTasks { get; private set; }

        public Event<OrchestratorStart> Started { get; private set; }
        public Event<ProjectCreatedEvent> ProjectCreated { get; private set; }

        public OrchestratorStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => Started, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => ProjectCreated, x => x.CorrelateById(m => m.Message.CorrelationId));

            Initially(
                When(Started)
                    .Then(ctx => { ctx.Instance.ProjectId = ctx.Data.ProjectId; ctx.Instance.StartedAt = DateTime.UtcNow; })
                    .TransitionTo(WaitingForTasks)
            );

            During(WaitingForTasks,
                When(ProjectCreated)
                    .Then(ctx => { /* orchestrator reacts to ProjectCreated and may trigger other actions */ })
                    .Finalize()
            );

            SetCompletedWhenFinalized();
        }
    }
}
