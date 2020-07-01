FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001
ENV ASPNETCORE_URLS=http://*:5000

FROM  mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["./", "./"]
COPY ["RatesAPI/RatesAPI.csproj", "./RatesAPI/"]
COPY ["Services/Services.csproj", "./Services/"]
COPY ["Core/Core.csproj", "./Core/"]
COPY ["Services.Tests/Services.Tests.csproj", "./Services.Tests/"]

RUN dotnet restore "./RatesAPI/RatesAPI.csproj"

COPY . .
COPY ["RatesAPI/.", "./RatesAPI/"]
COPY ["Services/.", "./Services/"]
COPY ["Core/.", "./Core/"]
WORKDIR "/src/."

RUN dotnet build "./RatesAPI/RatesAPI.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RatesAPI.dll"]