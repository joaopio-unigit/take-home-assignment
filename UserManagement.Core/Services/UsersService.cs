using UserManagement.Core.Interfaces;
using UserManagement.Core.Models;

namespace UserManagement.Core.Services;

public class UsersService : IUsersService
{
    private readonly List<User> _users;

    public UsersService(IUsersRepository repository)
    {
        _users = repository.GetAll().ToList();
    }

    public User? FindByEmail(string email) =>
        _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<User> FindOlderThan(int age) =>
        _users.Where(u => u.Age > age);

    public double GetAverageAge() =>
        _users.Count == 0 ? 0 : _users.Average(u => u.Age);
}