# Party Planner Backend

Backend .NET do aplicativo Party Planner, organizado em projetos separados dentro
de `src`.

## Estrutura

- `src/PartyPlanner.WebApi`
  API HTTP, controllers, Swagger e startup.
- `src/PartyPlanner.Application`
  Interfaces e services da aplicacao.
- `src/PartyPlanner.Core`
  Entidades, DTOs e extensoes compartilhadas do dominio.
- `src/PartyPlanner.Infrastructure`
  Entity Framework Core, repository, DbContext e migrations.
- `src/PartyPlanner.Common`
  Base comum para crescer o projeto sem acoplar camadas.

## Padrao adotado

- `Controller` recebe a requisicao HTTP
- `Service` orquestra o caso de uso
- `Repository` acessa a base via EF Core
- `Core` concentra entidades e contratos de dados

## Como rodar a API

```bash
dotnet run --project src\PartyPlanner.WebApi
```

No Visual Studio, abra [PartyPlanner.slnx](C:/Users/luizd/Documentos/Github/partyPlanner-backend/PartyPlanner.slnx).
Ao executar em `Development`, o navegador abre automaticamente no Swagger:

```text
http://localhost:5112/swagger
```

## PostgreSQL com Docker

Para subir o banco:

```bash
docker compose up -d
```

Configuracao padrao:

- host: `localhost`
- porta: `5433`
- usuario: `postgres`
- senha: `postgres`
- database: `partyplannerdb`

String de conexao:

```text
Host=localhost;Port=5433;Database=partyplannerdb;Username=postgres;Password=postgres;
```

## Entity Framework

Migration inicial gerada em:

- `src/PartyPlanner.Infrastructure/Migrations`

Para aplicar as migrations com `dotnet ef`:

```bash
dotnet ef database update --project src\PartyPlanner.Infrastructure\PartyPlanner.Infrastructure.csproj --startup-project src\PartyPlanner.WebApi\PartyPlanner.WebApi.csproj --context PartyPlannerDbContext
```

Para criar uma nova migration:

```bash
dotnet ef migrations add NOME_DA_MIGRATION --project src\PartyPlanner.Infrastructure\PartyPlanner.Infrastructure.csproj --startup-project src\PartyPlanner.WebApi\PartyPlanner.WebApi.csproj --context PartyPlannerDbContext --output-dir Migrations
```

No Visual Studio Package Manager Console, use:

```powershell
Add-Migration NOME_DA_MIGRATION -Project PartyPlanner.Infrastructure -StartupProject PartyPlanner.WebApi -Context PartyPlannerDbContext -OutputDir Migrations
Update-Database -Project PartyPlanner.Infrastructure -StartupProject PartyPlanner.WebApi -Context PartyPlannerDbContext
```
