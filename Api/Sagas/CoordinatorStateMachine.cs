using System;
using MassTransit;
using MassTransit.Saga;
using Automatonymous;

namespace Api.Sagas
{
    public record StartCoordinator(Guid CorrelationId, string ProjectId);
    public record ProjectCreated(Guid CorrelationId, string ProjectId);
    public record TaskCreated(Guid CorrelationId, string TaskId, string ProjectId);

    public class CoordinatorStateMachine : MassTransitStateMachine<CoordinatorState>
    {
        public State WaitingForProject { get; private set; }
        public State WaitingForTask { get; private set; }

        public Event<StartCoordinator> Started { get; private set; }
        public Event<ProjectCreated> ProjectCreatedEvent { get; private set; }
        public Event<TaskCreated> TaskCreatedEvent { get; private set; }

        public CoordinatorStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => Started, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => ProjectCreatedEvent, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => TaskCreatedEvent, x => x.CorrelateById(m => m.Message.CorrelationId));

            Initially(
                When(Started)
                    .Then(ctx =>
                    {
                        ctx.Instance.ProjectId = ctx.Data.ProjectId;
                        ctx.Instance.Created = DateTime.UtcNow;
                    })
                    .ThenAsync(ctx => ctx.Publish(new CreateProjectCommand(ctx.Instance.CorrelationId, ctx.Instance.ProjectId)))
                    .TransitionTo(WaitingForProject)
            );

            During(WaitingForProject,
                When(ProjectCreatedEvent)
                    .ThenAsync(ctx => ctx.Publish(new CreateTaskCommand(ctx.Instance.CorrelationId, Guid.NewGuid().ToString(), ctx.Instance.ProjectId)))
                    .TransitionTo(WaitingForTask)
            );

            During(WaitingForTask,
                When(TaskCreatedEvent)
                    .Then(ctx => { /* saga complete */ })
                    .Finalize()
            );

            SetCompletedWhenFinalized();
        }
    }

    // Commands published by the saga
    public record CreateProjectCommand(Guid CorrelationId, string ProjectId);
    public record CreateTaskCommand(Guid CorrelationId, string TaskId, string ProjectId);
}
