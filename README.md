# UniversityManagement

UniversityManagement models course configuration, student enrollment, prerequisites,
exams, discounts, payments, semester promotion, and academic/financial reporting.
The project intentionally has no user interface.

## Architecture

- `UniversityManagement.Domain` contains entities and object-level invariants.
- `UniversityManagement.Services` contains cross-entity validation and orchestration.
- `UniversityManagement.Data` contains EF Core 10 SQL Server persistence and repositories.

The solution targets .NET 10. Tests use xUnit, Moq, and coverlet; source code is
checked by StyleCop analyzers during the build.

## Build and test

```powershell
dotnet build UniversityManagement.slnx
dotnet test UniversityManagement.slnx --no-build
```

The Data test project runs real integration tests against the default SQL Server
instance on `localhost`. The account running the tests needs permission to create
and delete temporary databases named `UniversityManagementTests_<GUID>`.

## Coverage

```powershell
dotnet test UniversityManagement.Domain.Tests\UniversityManagement.Domain.Tests.csproj `
  --settings UniversityManagement.runsettings --collect:"XPlat Code Coverage"

dotnet test UniversityManagement.Services.Tests\UniversityManagement.Services.Tests.csproj `
  --settings UniversityManagement.runsettings --collect:"XPlat Code Coverage"
```

The current suite contains 232 test methods: 202 `[Fact]` methods and 30
`[Theory]` methods. Each theory is counted once, regardless of its inline data.
The latest measured line coverage is 91.53% for Domain and 98.88% for Services.
