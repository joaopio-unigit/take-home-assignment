using UserManagement.Core.Interfaces;
using UserManagement.Core.Repositories;
using UserManagement.Core.Services;

internal class Program
{
    private const string EmailLookup = "jane.smith@example.com";
    private const int AgeThreshold = 30;
    private const string CommandPrompt = "\nEnter a command (1-4): ";

    private static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.Error.WriteLine("Error: No CSV file path was supplied.");
            return 1;
        }

        var csvPath = args[0];
        if (!File.Exists(csvPath))
        {
            Console.Error.WriteLine($"Error: The CSV file was not found: {csvPath}");
            return 1;
        }

        IUsersService service;

        try
        {
            using var reader = new StreamReader(csvPath);
            var repository = new UsersRepository(reader);
            service = new UsersService(repository);
        }
        catch (Exception)
        {
            Console.Error.WriteLine("Error: The user data could not be loaded from the CSV file.");
            return 1;
        }

        PrintMenu(csvPath);

        while (true)
        {
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    FindByEmail(service);
                    break;

                case "2":
                    FindOlderThan(service);
                    break;

                case "3":
                    CalculateAverageAge(service);
                    break;

                case "4":
                    Console.WriteLine("Goodbye!");
                    return 0;

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

            Console.Write(CommandPrompt);
        }
    }

    private static void PrintMenu(string csvPath)
    {
        Console.WriteLine("User Management Console");
        Console.WriteLine("========================");
        Console.WriteLine($"Loaded data from: {csvPath}");
        Console.WriteLine();
        Console.WriteLine($"1. Find user with email {EmailLookup}");
        Console.WriteLine($"2. Find all users older than {AgeThreshold}");
        Console.WriteLine("3. Calculate average age of all users");
        Console.WriteLine("4. Exit");
        Console.Write(CommandPrompt);
    }

    private static void FindByEmail(IUsersService service)
    {
        var user = service.FindByEmail(EmailLookup);
        if (user is null)
        {
            Console.WriteLine($"User with email {EmailLookup} was not found.");
        }
        else
        {
            Console.WriteLine($"Id: {user.Id}");
            Console.WriteLine($"Name: {user.Name}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Age: {user.Age}");
        }
    }

    private static void FindOlderThan(IUsersService service)
    {
        var olderUsers = service.FindOlderThan(AgeThreshold).ToList();
        if (olderUsers.Count == 0)
        {
            Console.WriteLine($"No users older than {AgeThreshold} were found.");
        }
        else
        {
            foreach (var olderUser in olderUsers)
            {
                Console.WriteLine($"Id: {olderUser.Id}, Name: {olderUser.Name}, Email: {olderUser.Email}, Age: {olderUser.Age}");
            }
        }
    }

    private static void CalculateAverageAge(IUsersService service)
    {
        Console.WriteLine($"Average age: {service.GetAverageAge():F2}");
    }
}
