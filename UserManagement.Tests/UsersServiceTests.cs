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
        var users = TestData.CreateUsers();

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
        var users = TestData.CreateUsers();

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
        var users = TestData.CreateUsers();

        var mockRepository = new Mock<IUsersRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(users);

        var service = new UsersService(mockRepository.Object);

        var result = service.FindByEmail("missing@example.com");

        Assert.Null(result);
    }

    [Fact]
    public void FindOlderThan_ReturnsOnlyUsersStrictlyOlderThanAge()
    {
        var users = TestData.CreateUsers();

        var mockRepository = new Mock<IUsersRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(users);

        var service = new UsersService(mockRepository.Object);

        var result = service.FindOlderThan(30).ToList();

        Assert.Equal(3, result.Count);
        Assert.Contains(result, u => u.Id == 1);
        Assert.Contains(result, u => u.Id == 3);
        Assert.Contains(result, u => u.Id == 5);
        Assert.DoesNotContain(result, u => u.Id == 4);
    }

    [Fact]
    public void FindOlderThan_WhenNoUsersMatch_ReturnsEmptyCollection()
    {
        var users = TestData.CreateUsers();

        var mockRepository = new Mock<IUsersRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(users);

        var service = new UsersService(mockRepository.Object);

        var result = service.FindOlderThan(65).ToList();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAverageAge_WhenUsersExist_ReturnsCorrectAverage()
    {
        var users = TestData.CreateUsers();

        var mockRepository = new Mock<IUsersRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(users);

        var service = new UsersService(mockRepository.Object);

        Assert.Equal(38.2, service.GetAverageAge());
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

    [Fact]
    public void Constructor_RequestsUsersFromRepositoryExactlyOnce()
    {
        var mockRepository = new Mock<IUsersRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(TestData.CreateUsers());

        _ = new UsersService(mockRepository.Object);

        mockRepository.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public void MethodsUseCachedUsersWithoutRequestingRepositoryAgain()
    {
        var mockRepository = new Mock<IUsersRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(TestData.CreateUsers());
        var service = new UsersService(mockRepository.Object);

        service.FindByEmail("john.doe@example.com");
        service.FindOlderThan(30).ToList();
        service.GetAverageAge();

        mockRepository.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public void FindByEmail_WhenEmailIsEmpty_ReturnsNull()
    {
        var mockRepository = new Mock<IUsersRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(TestData.CreateUsers());
        var service = new UsersService(mockRepository.Object);

        var result = service.FindByEmail(string.Empty);

        Assert.Null(result);
    }

    [Fact]
    public void FindOlderThan_WhenAgeIsBelowAllUsers_ReturnsEveryUser()
    {
        var users = TestData.CreateUsers();
        var mockRepository = new Mock<IUsersRepository>();
        mockRepository.Setup(r => r.GetAll()).Returns(users);
        var service = new UsersService(mockRepository.Object);

        var result = service.FindOlderThan(0).ToList();

        Assert.Equal(users.Count, result.Count);
        Assert.Equal(users.Select(u => u.Id), result.Select(u => u.Id));
    }
}
