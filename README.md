# Party Planner Backend

API ASP.NET Core para o aplicativo Party Planner.

## Stack

- .NET 10
- ASP.NET Core Minimal API

## Estrutura

- `PartyPlanner.Api`: API principal
- `PartyPlanner.slnx`: solucao da API

## Endpoints iniciais

- `GET /health`
- `GET /api/parties`
- `GET /api/parties/{id}`
- `POST /api/parties`
- `POST /api/parties/{partyId}/tasks`
- `POST /api/parties/{partyId}/guests`
- `POST /api/parties/{partyId}/budget-items`

## Como rodar

```bash
dotnet run --project PartyPlanner.Api
```

## Proxima etapa recomendada

Adicionar persistencia com Entity Framework Core e conectar o app mobile.
