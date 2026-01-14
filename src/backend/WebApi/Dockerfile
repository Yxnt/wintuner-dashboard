FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/backend/WintunerDashboard.sln .
COPY src/backend/Domain/Domain.csproj src/backend/Domain/Domain.csproj
COPY src/backend/Application/Application.csproj src/backend/Application/Application.csproj
COPY src/backend/Infrastructure/Infrastructure.csproj src/backend/Infrastructure/Infrastructure.csproj
COPY src/backend/WebApi/WebApi.csproj src/backend/WebApi/WebApi.csproj
RUN dotnet restore src/backend/WebApi/WebApi.csproj
COPY src/backend src/backend
WORKDIR /src/src/backend/WebApi
RUN dotnet publish WebApi.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
