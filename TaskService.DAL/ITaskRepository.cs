using CoreLib.Models;
using System.Collections.Generic;

namespace TaskService.DAL
{
    public interface ITaskRepository
    {
        Task GetById(string id);
        IEnumerable<Task> GetAll(string projectId = null, string assigneeId = null);
        void Create(Task task);
        void Update(Task task);
        void Delete(string id);
        void Assign(string taskId, string assigneeId);
        IEnumerable<(Task, Project)> GetTasksWithProject(); // join
    }
}