#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 3000
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["HelloRadix/HelloRadix.csproj", "HelloRadix/"]
COPY ["HelloRadixTests/HelloRadixTests.csproj", "HelloRadixTests/"]
RUN dotnet restore "HelloRadix/HelloRadix.csproj"
RUN dotnet restore "HelloRadixTests/HelloRadixTests.csproj"
COPY . .

RUN dotnet build "HelloRadix/HelloRadix.csproj" -c Release -o /app/build
RUN dotnet build "HelloRadixTests/HelloRadixTests.csproj" -c Release -o /app/build

RUN dotnet test "HelloRadixTests/HelloRadixTests.csproj" /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:Threshold=10 /p:ThresholdType=line --logger "trx;LogFileName=webapplication1.trx" 

FROM build AS publish
RUN dotnet publish "HelloRadix/HelloRadix.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "HelloRadix.dll"]