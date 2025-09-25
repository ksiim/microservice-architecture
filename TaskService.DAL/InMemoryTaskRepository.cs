using CoreLib.Models;
using System.Collections.Generic;
using System.Linq;

namespace TaskService.DAL
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly List<Task> _tasks = new();
        private readonly IProjectRepository _projectRepo;

        public InMemoryTaskRepository(IProjectRepository projectRepo)
        {
            _projectRepo = projectRepo;
        }

        public Task GetById(string id) => _tasks.FirstOrDefault(t => t.Id == id);

        public IEnumerable<Task> GetAll(string projectId = null, string assigneeId = null)
        {
            return _tasks.Where(t =>
                (projectId == null || t.ProjectId == projectId) &&
                (assigneeId == null || t.AssigneeId == assigneeId));
        }

        public void Create(Task task)
        {
            _tasks.Add(task);
        }

        public void Update(Task task)
        {
            var idx = _tasks.FindIndex(t => t.Id == task.Id);
            if (idx >= 0) _tasks[idx] = task;
        }

        public void Delete(string id)
        {
            _tasks.RemoveAll(t => t.Id == id);
        }

        public void Assign(string taskId, string assigneeId)
        {
            var task = GetById(taskId);
            if (task != null) task.AssigneeId = assigneeId;
        }

        public IEnumerable<(Task, Project)> GetTasksWithProject()
        {
            var projects = _projectRepo.GetAll();
            return from t in _tasks
                   join p in projects on t.ProjectId equals p.Id
                   select (t, p);
        }
    }
}