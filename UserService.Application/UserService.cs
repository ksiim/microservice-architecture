using UserService.Core;
using UserService.Core.Entities;

namespace UserService.Application
{
    public class UserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public User? GetUser(Guid id) => _repo.GetById(id);
        public IEnumerable<User> GetAllUsers() => _repo.GetAll();
        public void CreateUser(User user) => _repo.Create(user);
        public void UpdateUser(User user) => _repo.Update(user);
        public void DeleteUser(Guid id) => _repo.Delete(id);
        public User? GetByUsername(string username) => _repo.GetByUsername(username);
    }
}
