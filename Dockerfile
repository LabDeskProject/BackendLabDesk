# Build the API and the one-shot migration runner.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore src/Host/Host.csproj
RUN dotnet restore tools/DatabaseMigrator/DatabaseMigrator.csproj
RUN dotnet publish src/Host/Host.csproj -c Release -o /out/api --no-restore
RUN dotnet publish tools/DatabaseMigrator/DatabaseMigrator.csproj -c Release -o /out/migrator --no-restore

# Runtime image shared by the API and migration service.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /out/api/ ./
COPY --from=build /out/migrator/ ./migrator/
ENTRYPOINT ["dotnet", "Host.dll"]
