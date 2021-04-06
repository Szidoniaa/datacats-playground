#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 3000
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Equinor.OmniaDataCatalogApi.Api/Equinor.OmniaDataCatalogApi.Api.csproj", "Equinor.OmniaDataCatalogApi.Api/"]
COPY ["Equinor.OmniaDataCatalogApi.Collibra/Equinor.OmniaDataCatalogApi.Collibra.csproj", "Equinor.OmniaDataCatalogApi.Collibra/"]
COPY ["Equinor.OmniaDataCatalogApi.Common/Equinor.OmniaDataCatalogApi.Common.csproj", "Equinor.OmniaDataCatalogApi.Common/"]
COPY ["Equinor.OmniaDataCatalogApi.UnitTests/Equinor.OmniaDataCatalogApi.UnitTests.csproj", "Equinor.OmniaDataCatalogApi.UnitTests/"]

RUN dotnet restore "Equinor.OmniaDataCatalogApi.Api/Equinor.OmniaDataCatalogApi.Api.csproj"
RUN dotnet restore "Equinor.OmniaDataCatalogApi.Collibra/Equinor.OmniaDataCatalogApi.Collibra.csproj"
RUN dotnet restore "Equinor.OmniaDataCatalogApi.Common/Equinor.OmniaDataCatalogApi.Common.csproj"
RUN dotnet restore "Equinor.OmniaDataCatalogApi.UnitTests/Equinor.OmniaDataCatalogApi.UnitTests.csproj"
COPY . .

RUN dotnet build "Equinor.OmniaDataCatalogApi.Api/Equinor.OmniaDataCatalogApi.Api.csproj" -c Release -o /app/build
RUN dotnet build "Equinor.OmniaDataCatalogApi.Collibra/Equinor.OmniaDataCatalogApi.Collibra.csproj" -c Release -o /app/build
RUN dotnet build "Equinor.OmniaDataCatalogApi.Common/Equinor.OmniaDataCatalogApi.Common.csproj" -c Release -o /app/build
RUN dotnet build "Equinor.OmniaDataCatalogApi.UnitTests/Equinor.OmniaDataCatalogApi.UnitTests.csproj" -c Release -o /app/build

RUN dotnet test "Equinor.OmniaDataCatalogApi.UnitTests/Equinor.OmniaDataCatalogApi.UnitTests.csproj" /p:CollectCoverage=true  /p:Threshold=5 /p:ThresholdType=line /p:CoverletOutputFormat=cobertura --logger "trx;LogFileName=webapplication1.trx" 

FROM build AS publish
RUN dotnet publish "Equinor.OmniaDataCatalogApi.Api/Equinor.OmniaDataCatalogApi.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Equinor.OmniaDataCatalogApi.Api.dll"]