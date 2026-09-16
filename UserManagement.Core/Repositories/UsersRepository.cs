using System.Globalization;
using CsvHelper;
using UserManagement.Core.Interfaces;
using UserManagement.Core.Models;

namespace UserManagement.Core.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly List<User> _users;

    public UsersRepository(TextReader reader)
    {
        try
        {
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            _users = csv.GetRecords<User>().ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("The user data could not be loaded from the CSV file.", ex);
        }
    }

    public IEnumerable<User> GetAll() => _users;
}