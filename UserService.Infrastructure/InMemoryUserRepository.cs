using UserService.Core;
using UserService.Core.Entities;

namespace UserService.Infrastructure
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public User? GetById(Guid id) => _users.FirstOrDefault(u => u.Id == id);
        public IEnumerable<User> GetAll() => _users;
        public void Create(User user) => _users.Add(user);
        public void Update(User user)
        {
            var idx = _users.FindIndex(u => u.Id == user.Id);
            if (idx >= 0) _users[idx] = user;
        }
        public void Delete(Guid id) => _users.RemoveAll(u => u.Id == id);
        public User? GetByUsername(string username) => _users.FirstOrDefault(u => u.Username == username);
    }
}
