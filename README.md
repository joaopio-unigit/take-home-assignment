# User Management

Small .NET 10 console application that reads users from a CSV file and supports email lookup, age filtering, and average-age calculation.

## Prerequisites

- .NET 10 SDK

## Build and test

From the repository root:

```powershell
dotnet restore UserManagement.sln
dotnet build UserManagement.sln
dotnet test UserManagement.sln
```

## Run

Pass the CSV path as the first argument:

```powershell
dotnet run --project UserManagement.Console -- users.csv
```

or 
```powershell
cd UserManagement.Console
dotnet run -- ..\users.csv
```

The console application reads the CSV once at startup and then provides the four menu commands.

## Structure

- `UserManagement.Core/` - models, interfaces, repository, and service logic
- `UserManagement.Console/` - console application entry point
- `UserManagement.Tests/` - xUnit tests and Moq-based service tests
- `users.csv` - sample input data
