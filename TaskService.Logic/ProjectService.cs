using CoreLib.Models;
using TaskService.DAL;
using System.Collections.Generic;

namespace TaskService.Logic
{
    public class ProjectService
    {
        private readonly IProjectRepository _projectRepo;
        public ProjectService(IProjectRepository projectRepo)
        {
            _projectRepo = projectRepo;
        }

        public Project GetProject(string id) => _projectRepo.GetById(id);
        public IEnumerable<Project> GetProjects() => _projectRepo.GetAll();
        public void CreateProject(Project project) => _projectRepo.Create(project);
        public void UpdateProject(Project project) => _projectRepo.Update(project);
        public void DeleteProject(string id) => _projectRepo.Delete(id);
    }
}