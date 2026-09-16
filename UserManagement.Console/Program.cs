using UserManagement.Core.Interfaces;
using UserManagement.Core.Repositories;
using UserManagement.Core.Services;

const string EmailLookup = "jane.smith@example.com";
const int AgeThreshold = 30;

if (args.Length == 0)
{
    Console.Error.WriteLine("Error: No CSV file path was supplied.");
    Environment.Exit(1);
}

var csvPath = args[0];
if (!File.Exists(csvPath))
{
    Console.Error.WriteLine($"Error: The CSV file was not found: {csvPath}");
    Environment.Exit(1);
}

IUsersService? service = null;

try
{
    using var reader = new StreamReader(csvPath);
    var repository = new UsersRepository(reader);
    service = new UsersService(repository);
}
catch (Exception)
{
    Console.Error.WriteLine("Error: The user data could not be loaded from the CSV file.");
    Environment.Exit(1);
}

Console.WriteLine("User Management Console");
Console.WriteLine("========================");
Console.WriteLine($"Loaded data from: {csvPath}");
Console.WriteLine();
Console.WriteLine("1. Find user with email jane.smith@example.com");
Console.WriteLine("2. Find all users older than 30");
Console.WriteLine("3. Calculate average age of all users");
Console.WriteLine("4. Exit");
Console.WriteLine();
Console.WriteLine("Enter a command (1-4):");

while (true)
{
    Console.Write("Command: ");
    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            var user = service!.FindByEmail(EmailLookup);
            if (user is null)
            {
                Console.WriteLine("User with email jane.smith@example.com was not found.");
            }
            else
            {
                Console.WriteLine($"Id: {user.Id}");
                Console.WriteLine($"Name: {user.Name}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Age: {user.Age}");
            }
            break;

        case "2":
            var olderUsers = service!.FindOlderThan(AgeThreshold).ToList();
            if (olderUsers.Count == 0)
            {
                Console.WriteLine("No users older than 30 were found.");
            }
            else
            {
                foreach (var olderUser in olderUsers)
                {
                    Console.WriteLine($"Id: {olderUser.Id}, Name: {olderUser.Name}, Email: {olderUser.Email}, Age: {olderUser.Age}");
                }
            }
            break;

        case "3":
            Console.WriteLine($"Average age: {service!.GetAverageAge():F2}");
            break;

        case "4":
            Console.WriteLine("Goodbye!");
            return;

        default:
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("No command was entered. Please enter a number between 1 and 4.");
            }
            else
            {
                Console.WriteLine("Unknown command. Please enter a number between 1 and 4.");
            }
            break;
    }

    Console.WriteLine("Enter a command (1-4):");
}
