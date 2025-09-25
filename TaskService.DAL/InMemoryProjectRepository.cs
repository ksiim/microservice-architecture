using CoreLib.Models;
using System.Collections.Generic;
using System.Linq;

namespace TaskService.DAL
{
    public class InMemoryProjectRepository : IProjectRepository
    {
        private readonly List<Project> _projects = new();

        public Project GetById(string id) => _projects.FirstOrDefault(p => p.Id == id);

        public IEnumerable<Project> GetAll() => _projects;

        public void Create(Project project)
        {
            _projects.Add(project);
        }

        public void Update(Project project)
        {
            var idx = _projects.FindIndex(p => p.Id == project.Id);
            if (idx >= 0) _projects[idx] = project;
        }

        public void Delete(string id)
        {
            _projects.RemoveAll(p => p.Id == id);
        }
    }
}