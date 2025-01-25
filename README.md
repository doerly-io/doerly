## How to create a migration
`cd 'YourProjectPath with DbContext'`<br />

migration example:<br />
`dotnet ef migrations add Initial --project Doerly.Module.Authorization.DataAccess.csproj --startup-project ..\..\Doerly.Host\Doerly.Host.csproj --context Doerly.Module.Authorization.DataAccess.AuthorizationDbContext --output-dir Migrations`

## Docker compose command
`docker-compose up -d`

## Description, plans and considerations

Each module should be implemented with consideration of low coupling with other modules.
Thus, each module shouldn't have direct dependencies on other modules.
Async communication should be implemented with the help of the message broker.
Sync communication ideally should be implemented with the help of the HTTP API calls, but for faster development on early stages 
is implemented by using module Interface contracts through Client assemblies.
Client assemblies are common to all modules and describe contract for communicating with module. In the future, it should be replaced with HTTP API calls.

Each module should have its own database schema (in future replaced with DB per module).

The example of Authorization module is implemented with the following architecture:
- Doerly.Module.Authorization.Api - HTTP API for Authorization module
- Doerly.Module.Authorization.Domain - Domain layer for Authorization module with business logic
- Doerly.Module.Authorization.DataAccess - Data access layer for Authorization module
- Doerly.Module.Authorization.Localization - Localization for Authorization module

all other modules should be implemented with the same architecture.
**But**
When complex business logic is required, module architecture should be done in a following way:
- Doerly.Module.ModuleName.Api - HTTP API for ModuleName module
- Doerly.Module.ModuleName.Application (or Handlers/UseCases) - Application layer for use cases that should have Handlers/UseCases which are responsible for orchestration of business logic
- Doerly.Module.ModuleName.Domain - Domain layer for ModuleName module with business logic separated in single Command/Processor classes
- Doerly.Module.ModuleName.DataAccess - Data access layer for ModuleName module
- Doerly.Module.ModuleName.Localization - Localization for ModuleName module

***Note: architecture description for messaging assemblies will be added when they will be needed***









