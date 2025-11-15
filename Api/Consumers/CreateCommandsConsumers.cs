using System.Threading.Tasks;
using MassTransit;
using Api.Sagas;
using TaskService.Logic;
using TaskModel = CoreLib.Models.Task;
using ProjectModel = CoreLib.Models.Project;

namespace Api.Consumers
{
    public class CreateProjectConsumer : IConsumer<CreateProjectCommand>
    {
        private readonly TaskService.Logic.ProjectService _projectService;

        public CreateProjectConsumer(TaskService.Logic.ProjectService projectService)
        {
            _projectService = projectService;
        }

        public Task Consume(ConsumeContext<CreateProjectCommand> context)
        {
            var cmd = context.Message;
            // create project locally
            _projectService.CreateProject(new ProjectModel { Id = cmd.ProjectId, Name = "FromSagaProject" });
            // publish ProjectCreated
            return context.Publish(new ProjectCreated(cmd.CorrelationId, cmd.ProjectId));
        }
    }

    public class CreateTaskConsumer : IConsumer<CreateTaskCommand>
    {
        private readonly TaskService.Logic.TaskService _taskService;

        public CreateTaskConsumer(TaskService.Logic.TaskService taskService)
        {
            _taskService = taskService;
        }

        public Task Consume(ConsumeContext<CreateTaskCommand> context)
        {
            var cmd = context.Message;
            _taskService.CreateTask(new TaskModel { Id = cmd.TaskId, Title = "FromSagaTask", ProjectId = cmd.ProjectId });
            return context.Publish(new TaskCreated(cmd.CorrelationId, cmd.TaskId, cmd.ProjectId));
        }
    }
}
