using UserService.Core.Entities;

namespace UserService.Core
{
    public interface IUserRepository
    {
        User? GetById(Guid id);
        IEnumerable<User> GetAll();
        void Create(User user);
        void Update(User user);
        void Delete(Guid id);
        User? GetByUsername(string username);
    }
}
