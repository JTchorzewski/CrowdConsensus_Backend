FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CrowdConsensus_API.sln ./CrowdConsensus_API.sln
COPY CrowdConsensus_API/ ./CrowdConsensus_API/
COPY CC_Application/ ./CC_Application/
COPY CC_Domain/ ./CC_Domain/
COPY CC_Infrastructure/ ./CC_Infrastructure/

RUN dotnet restore CrowdConsensus_API.sln

WORKDIR /src/CC_Application
RUN dotnet publish Application.csproj --self-contained --runtime linux-musl-x64 --configuration Release --output /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CC_Application.dll"]
