FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/PartyPlanner.Common/PartyPlanner.Common.csproj src/PartyPlanner.Common/
COPY src/PartyPlanner.Core/PartyPlanner.Core.csproj src/PartyPlanner.Core/
COPY src/PartyPlanner.Application/PartyPlanner.Application.csproj src/PartyPlanner.Application/
COPY src/PartyPlanner.Infrastructure/PartyPlanner.Infrastructure.csproj src/PartyPlanner.Infrastructure/
COPY src/PartyPlanner.WebApi/PartyPlanner.WebApi.csproj src/PartyPlanner.WebApi/
RUN dotnet restore src/PartyPlanner.WebApi/PartyPlanner.WebApi.csproj

COPY src ./src
RUN dotnet publish src/PartyPlanner.WebApi/PartyPlanner.WebApi.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PartyPlanner.WebApi.dll"]
