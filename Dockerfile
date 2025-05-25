FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Skopiuj tylko plik .sln i pliki .csproj potrzebne do przywrócenia zależności
COPY *.sln .
COPY CrowdConsensus_API/*.csproj ./CrowdConsensus_API/
COPY CC_Domain/*.csproj ./CC_Domain/
COPY CC_Application/*.csproj ./CC_Application/
COPY CC_Infrastructure/*.csproj ./CC_Infrastructure/

# Przywróć zależności
RUN dotnet restore CrowdConsensus_API/CrowdConsensus_API.csproj

# Skopiuj resztę kodu źródłowego
COPY CrowdConsensus_API/. ./CrowdConsensus_API/
COPY CC_Domain/. ./CC_Domain/
COPY CC_Application/. ./CC_Application/
COPY CC_Infrastructure/. ./CC_Infrastructure/

WORKDIR /src/CrowdConsensus_API
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CrowdConsensus_API.dll"]