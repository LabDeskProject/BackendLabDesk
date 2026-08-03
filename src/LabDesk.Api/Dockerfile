# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/LabDesk.Api/LabDesk.Api.csproj", "src/LabDesk.Api/"]
COPY ["src/Module/Identity/LabDesk.Modules.Identity.Application/LabDesk.Modules.Identity.Application.csproj", "src/Module/Identity/LabDesk.Modules.Identity.Application/"]
COPY ["src/SeedWork/LabDesk.SeedWork.Application/LabDesk.SeedWork.Application.csproj", "src/SeedWork/LabDesk.SeedWork.Application/"]
COPY ["src/SeedWork/LabDesk.SeedWork.Domain/LabDesk.SeedWork.Domain.csproj", "src/SeedWork/LabDesk.SeedWork.Domain/"]
COPY ["src/Module/Identity/LabDesk.Modules.Identity.Domain/LabDesk.Modules.Identity.Domain.csproj", "src/Module/Identity/LabDesk.Modules.Identity.Domain/"]
COPY ["src/Module/Identity/LabDesk.Modules.Identity.Infrastructure/LabDesk.Modules.Identity.Infrastructure.csproj", "src/Module/Identity/LabDesk.Modules.Identity.Infrastructure/"]
COPY ["src/SeedWork/LabDesk.SeedWork.Infrastructure/LabDesk.SeedWork.Infrastructure.csproj", "src/SeedWork/LabDesk.SeedWork.Infrastructure/"]
COPY ["src/Module/Identity/LabDesk.Modules.Identity.Presentation/LabDesk.Modules.Identity.Presentation.csproj", "src/Module/Identity/LabDesk.Modules.Identity.Presentation/"]
RUN dotnet restore "./src/LabDesk.Api/LabDesk.Api.csproj"
COPY . .
WORKDIR "/src/src/LabDesk.Api"
RUN dotnet build "./LabDesk.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LabDesk.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LabDesk.Api.dll"]