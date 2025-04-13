# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CrowdConsensus_API/CrowdConsensus_API.csproj", "CrowdConsensus_API/"]
COPY ["CC_Application/Application.csproj", "CC_Application/"]
COPY ["CC_Infrastructure/Infrastructure.csproj", "CC_Infrastructure/"]
COPY ["CC_Domain/Domain.csproj", "CC_Domain/"]
RUN dotnet restore "./CrowdConsensus_API/CrowdConsensus_API.csproj"
COPY . .
WORKDIR "/src/CrowdConsensus_API"
RUN dotnet build "./CrowdConsensus_API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CrowdConsensus_API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CrowdConsensus_API.dll"]