using UserManagement.Core.Interfaces;
using UserManagement.Core.Repositories;
using UserManagement.Core.Services;

const string EMAIL_LOOKUP = "jane.smith@example.com";
const int AGE_THRESHOLD = 30;
const string COMMAND_PROMPT = "\nEnter a command (1-4): ";

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
Console.WriteLine($"1. Find user with email {EMAIL_LOOKUP}");
Console.WriteLine($"2. Find all users older than {AGE_THRESHOLD}");
Console.WriteLine("3. Calculate average age of all users");
Console.WriteLine("4. Exit");
Console.Write(COMMAND_PROMPT);

while (true)
{
    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            FindByEmail(service!);
            break;

        case "2":
            FindOlderThan(service!);
            break;

        case "3":
            CalculateAverageAge(service!);
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

    Console.Write(COMMAND_PROMPT);
}

static void FindByEmail(IUsersService service)
{
    var user = service.FindByEmail(EMAIL_LOOKUP);
    if (user is null)
    {
        Console.WriteLine($"User with email {EMAIL_LOOKUP} was not found.");
    }
    else
    {
        Console.WriteLine($"Id: {user.Id}");
        Console.WriteLine($"Name: {user.Name}");
        Console.WriteLine($"Email: {user.Email}");
        Console.WriteLine($"Age: {user.Age}");
    }
}

static void FindOlderThan(IUsersService service)
{
    var olderUsers = service.FindOlderThan(AGE_THRESHOLD).ToList();
    if (olderUsers.Count == 0)
    {
        Console.WriteLine($"No users older than {AGE_THRESHOLD} were found.");
    }
    else
    {
        foreach (var olderUser in olderUsers)
        {
            Console.WriteLine($"Id: {olderUser.Id}, Name: {olderUser.Name}, Email: {olderUser.Email}, Age: {olderUser.Age}");
        }
    }
}

static void CalculateAverageAge(IUsersService service)
{
    Console.WriteLine($"Average age: {service.GetAverageAge():F2}");
}
