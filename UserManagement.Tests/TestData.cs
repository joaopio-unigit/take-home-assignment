using UserManagement.Core.Models;

namespace UserManagement.Tests;

internal static class TestData
{
    public static List<User> CreateUsers() =>
    [
        new() { Id = 1, Name = "John Doe", Email = "john.doe@example.com", Age = 31 },
        new() { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com", Age = 25 },
        new() { Id = 3, Name = "Bob Johnson", Email = "bob.johnson@example.com", Age = 40 },
        new() { Id = 4, Name = "Alice Brown", Email = "alice.brown@example.com", Age = 30 },
        new() { Id = 5, Name = "Carlos Garcia", Email = "carlos.garcia@example.com", Age = 65 }
    ];

    public static string CreateCsv() =>
        "Id,Name,Email,Age\n" +
        "1,John Doe,john.doe@example.com,31\n" +
        "2,Jane Smith,jane.smith@example.com,25\n" +
        "3,Bob Johnson,bob.johnson@example.com,40\n" +
        "4,Alice Brown,alice.brown@example.com,30\n" +
        "5,Carlos Garcia,carlos.garcia@example.com,65\n";
}
