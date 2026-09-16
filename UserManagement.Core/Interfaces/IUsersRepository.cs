using UserManagement.Core.Models;

namespace UserManagement.Core.Interfaces;

public interface IUsersRepository
{
    IEnumerable<User> GetAll();
}