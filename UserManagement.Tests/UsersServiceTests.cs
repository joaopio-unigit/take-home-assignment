using Moq;
using UserManagement.Core.Interfaces;
using UserManagement.Core.Models;
using UserManagement.Core.Services;

namespace UserManagement.Tests;

public class UsersServiceTests
{
    [Fact]
    public void FindByEmail_WhenEmailExists_ReturnsCorrectUser()
    {
        var users = new List<User>
        {
            new() { Id = 1, Name = "John Doe", Email = "john.doe@example.com", Age = 31 },
            new() { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com", Age = 25 }
        };

        var mockRepository = new Mock<IUsersRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(users);

        var service = new UsersService(mockRepository.Object);

        var result = service.FindByEmail("jane.smith@example.com");

        Assert.NotNull(result);
        Assert.Equal(2, result!.Id);
        Assert.Equal("Jane Smith", result.Name);
    }

    [Fact]
    public void FindByEmail_WhenEmailExistsWithDifferentCasing_ReturnsUser()
    {
        var users = new List<User>
        {
            new() { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com", Age = 25 }
        };

        var mockRepository = new Mock<IUsersRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(users);

        var service = new UsersService(mockRepository.Object);

        var result = service.FindByEmail("JANE.SMITH@EXAMPLE.COM");

        Assert.NotNull(result);
        Assert.Equal("Jane Smith", result!.Name);
    }

    [Fact]
    public void FindByEmail_WhenEmailDoesNotExist_ReturnsNull()
    {
        var users = new List<User>
        {
            new() { Id = 1, Name = "John Doe", Email = "john.doe@example.com", Age = 31 }
        };

        var mockRepository = new Mock<IUsersRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(users);

        var service = new UsersService(mockRepository.Object);

        var result = service.FindByEmail("missing@example.com");

        Assert.Null(result);
    }

    [Fact]
    public void FindOlderThan_ReturnsOnlyUsersStrictlyOlderThanAge()
    {
        var users = new List<User>
        {
            new() { Id = 1, Name = "John Doe", Email = "john.doe@example.com", Age = 31 },
            new() { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com", Age = 30 },
            new() { Id = 3, Name = "Bob Johnson", Email = "bob.johnson@example.com", Age = 40 }
        };

        var mockRepository = new Mock<IUsersRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(users);

        var service = new UsersService(mockRepository.Object);

        var result = service.FindOlderThan(30).ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.Id == 1);
        Assert.Contains(result, u => u.Id == 3);
        Assert.DoesNotContain(result, u => u.Id == 2);
    }

    [Fact]
    public void FindOlderThan_WhenNoUsersMatch_ReturnsEmptyCollection()
    {
        var users = new List<User>
        {
            new() { Id = 1, Name = "Jane Smith", Email = "jane.smith@example.com", Age = 30 },
            new() { Id = 2, Name = "Bob Johnson", Email = "bob.johnson@example.com", Age = 30 }
        };

        var mockRepository = new Mock<IUsersRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(users);

        var service = new UsersService(mockRepository.Object);

        var result = service.FindOlderThan(30).ToList();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAverageAge_WhenUsersExist_ReturnsCorrectAverage()
    {
        var users = new List<User>
        {
            new() { Id = 1, Name = "John Doe", Email = "john.doe@example.com", Age = 30 },
            new() { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com", Age = 35 }
        };

        var mockRepository = new Mock<IUsersRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(users);

        var service = new UsersService(mockRepository.Object);

        Assert.Equal(32.5, service.GetAverageAge());
    }

    [Fact]
    public void GetAverageAge_WhenListIsEmpty_ReturnsZero()
    {
        var users = new List<User>();

        var mockRepository = new Mock<IUsersRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(users);

        var service = new UsersService(mockRepository.Object);

        Assert.Equal(0, service.GetAverageAge());
    }
}
