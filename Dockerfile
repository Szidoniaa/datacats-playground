#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 3000
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["HelloRadix/HelloRadix.csproj", "HelloRadix/"]
COPY ["Collibra/Collibra.csproj", "Collibra/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["UnitTests/UnitTests.csproj", "UnitTests/"]

RUN dotnet restore "HelloRadix/HelloRadix.csproj"
RUN dotnet restore "Collibra/Collibra.csproj"
RUN dotnet restore "Common/Common.csproj"
RUN dotnet restore "UnitTests/UnitTests.csproj"
COPY . .

RUN dotnet build "HelloRadix/HelloRadix.csproj" -c Release -o /app/build
RUN dotnet build "Collibra/Collibra.csproj" -c Release -o /app/build
RUN dotnet build "Common/Common.csproj" -c Release -o /app/build
RUN dotnet build "UnitTests/UnitTests.csproj" -c Release -o /app/build

RUN dotnet test "UnitTests/UnitTests.csproj" /p:CollectCoverage=true  /p:Threshold=5 /p:ThresholdType=line /p:CoverletOutputFormat=cobertura --logger "trx;LogFileName=webapplication1.trx" 

FROM build AS publish
RUN dotnet publish "HelloRadix/HelloRadix.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "HelloRadix.dll"]