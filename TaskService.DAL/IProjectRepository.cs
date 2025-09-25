using CoreLib.Models;
using System.Collections.Generic;

namespace TaskService.DAL
{
    public interface IProjectRepository
    {
        Project GetById(string id);
        IEnumerable<Project> GetAll();
        void Create(Project project);
        void Update(Project project);
        void Delete(string id);
    }
}