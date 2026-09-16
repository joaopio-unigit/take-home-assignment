using System.IO;
using UserManagement.Core.Repositories;

namespace UserManagement.Tests;

public class UsersRepositoryTests
{
    [Fact]
    public void ValidCsvParsesCorrectly()
    {
        //MOCK CSV DATA
        var csv = "Id,Name,Email,Age\n1,John Doe,john.doe@example.com,31\n2,Jane Smith,jane.smith@example.com,25\n3,Bob Johnson,bob.johnson@example.com,40\n";

        var repository = new UsersRepository(new StringReader(csv));
        var users = repository.GetAll().ToList();

        Assert.Equal(3, users.Count);
        Assert.Equal(1, users[0].Id);
        Assert.Equal("John Doe", users[0].Name);
        Assert.Equal("john.doe@example.com", users[0].Email);
        Assert.Equal(31, users[0].Age);
        Assert.Equal(2, users[1].Id);
        Assert.Equal("Jane Smith", users[1].Name);
        Assert.Equal("jane.smith@example.com", users[1].Email);
        Assert.Equal(25, users[1].Age);
        Assert.Equal(3, users[2].Id);
        Assert.Equal("Bob Johnson", users[2].Name);
        Assert.Equal("bob.johnson@example.com", users[2].Email);
        Assert.Equal(40, users[2].Age);
    }

    [Fact]
    public void MalformedRowThrowsOnConstruction()
    {
        var csv = "Id,Name,Email,Age\n1,John Doe,john.doe@example.com,invalid\n";

        Assert.Throws<Exception>(() => new UsersRepository(new StringReader(csv)));
    }
}
