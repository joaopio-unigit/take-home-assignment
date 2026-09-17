using UserManagement.Core.Interfaces;
using UserManagement.Core.Repositories;
using UserManagement.Core.Services;

namespace UserManagement.Web;

public class UsersDataContext
{
    public bool IsLoaded { get; }
    public string? ErrorMessage { get; }
    public IUsersService? Service { get; }

    public UsersDataContext(IConfiguration configuration)
    {
        try
        {
            var csvPath = configuration["CsvFilePath"];

            if (string.IsNullOrWhiteSpace(csvPath))
            {
                ErrorMessage = "Error: No CSV file path was configured (CsvFilePath is missing).";
                IsLoaded = false;
                return;
            }

            if (!File.Exists(csvPath))
            {
                ErrorMessage = $"Error: The CSV file was not found: {csvPath}";
                IsLoaded = false;
                return;
            }

            using var reader = new StreamReader(csvPath);
            var repository = new UsersRepository(reader);
            Service = new UsersService(repository);
            IsLoaded = true;
        }
        catch (Exception)
        {
            ErrorMessage = "Error: The user data could not be loaded from the CSV file.";
            IsLoaded = false;
        }
    }
}
