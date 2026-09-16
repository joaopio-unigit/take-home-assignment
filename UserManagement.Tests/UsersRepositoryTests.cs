using System.IO;
using UserManagement.Core.Repositories;

namespace UserManagement.Tests;

public class UsersRepositoryTests
{
    [Fact]
    public void ValidCsvParsesCorrectly()
    {
        var csv = TestData.CreateCsv();

        var repository = new UsersRepository(new StringReader(csv));
        var users = repository.GetAll().ToList();

        Assert.Equal(5, users.Count);
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
        Assert.Equal(4, users[3].Id);
        Assert.Equal("Alice Brown", users[3].Name);
        Assert.Equal("alice.brown@example.com", users[3].Email);
        Assert.Equal(30, users[3].Age);
        Assert.Equal(5, users[4].Id);
        Assert.Equal("Carlos Garcia", users[4].Name);
        Assert.Equal("carlos.garcia@example.com", users[4].Email);
        Assert.Equal(65, users[4].Age);
    }

    [Fact]
    public void MalformedRowThrowsOnConstruction()
    {
        var csv = "Id,Name,Email,Age\n1,John Doe,john.doe@example.com,invalid\n";

        Assert.Throws<Exception>(() => new UsersRepository(new StringReader(csv)));
    }

    [Fact]
    public void MissingHeaderThrowsOnConstruction()
    {
        var csv = "Id,Name,Email,Years\n1,John Doe,john.doe@example.com,31\n";

        Assert.Throws<Exception>(() => new UsersRepository(new StringReader(csv)));
    }

    [Fact]
    public void EmptyCsvReturnsEmptyCollection()
    {
        var repository = new UsersRepository(new StringReader("Id,Name,Email,Age\n"));

        Assert.Empty(repository.GetAll());
    }

    [Fact]
    public void GetAllReturnsParsedUsersWithoutReloadingReader()
    {
        var repository = new UsersRepository(new StringReader(TestData.CreateCsv()));

        var firstRead = repository.GetAll().ToList();
        var secondRead = repository.GetAll().ToList();

        Assert.Equal(firstRead, secondRead);
    }
}
