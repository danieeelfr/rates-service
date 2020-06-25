FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copiar csproj e restaurar dependencias
COPY ./RatesAPI/RatesAPI.csproj ./
RUN dotnet restore

RUN pwsh -Command Write-Host "RatesAPI: Building Docker image..."

# Build da aplicacao
COPY . ./
RUN dotnet publish -c Release -o out

# Build da imagem
FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "RatesAPI.dll"]