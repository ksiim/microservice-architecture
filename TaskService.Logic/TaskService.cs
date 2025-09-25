using CoreLib.Models;
using TaskService.DAL;
using System.Collections.Generic;

namespace TaskService.Logic
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepo;
        public TaskService(ITaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public Task GetTask(string id) => _taskRepo.GetById(id);
        public IEnumerable<Task> GetTasks(string projectId = null, string assigneeId = null) => _taskRepo.GetAll(projectId, assigneeId);
        public void CreateTask(Task task) => _taskRepo.Create(task);
        public void UpdateTask(Task task) => _taskRepo.Update(task);
        public void DeleteTask(string id) => _taskRepo.Delete(id);
        public void AssignTask(string taskId, string assigneeId) => _taskRepo.Assign(taskId, assigneeId);
        public IEnumerable<(Task, Project)> GetTasksWithProject() => _taskRepo.GetTasksWithProject();
    }
}