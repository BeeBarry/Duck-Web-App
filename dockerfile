# Build-steg
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopierar endast projektfiler först för att utnyttja Docker-cache bättre
COPY ["*.sln", "./"]
COPY ["Duck.Core/*.csproj", "Duck.Core/"]
COPY ["Duck.Infrastructure/*.csproj", "Duck.Infrastructure/"]
COPY ["Duck.Api/*.csproj", "Duck.Api/"]

# Återställer NuGet-paket
RUN dotnet restore

# Kopierar resten av koden för backend-projekten
COPY Duck.Core/. Duck.Core/
COPY Duck.Infrastructure/. Duck.Infrastructure/
COPY Duck.Api/. Duck.Api/

# Bygger projektet
WORKDIR "/src/Duck.Api"
RUN dotnet build -c Release -o /app/build

# Publicerar
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Slutgiltigt steg
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Exponerar API-porten - jag har ändrat till 8080
EXPOSE 8080

# Skapar databasmapp
RUN mkdir -p /app/Data 

ENTRYPOINT ["dotnet", "Duck.Api.dll"]