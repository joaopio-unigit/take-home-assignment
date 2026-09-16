using UserManagement.Core.Models;

namespace UserManagement.Core.Interfaces;

public interface IUsersService
{
    User? FindByEmail(string email);
    IEnumerable<User> FindOlderThan(int age);
    double GetAverageAge();
}