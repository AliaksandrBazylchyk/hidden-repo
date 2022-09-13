#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/CatalogService/CatalogService.csproj", "Services/CatalogService/"]
COPY ["Data/DatabaseExtensions/DatabaseExtensions.csproj", "Data/DatabaseExtensions/"]
COPY ["Data/Models/Models.csproj", "Data/Models/"]
COPY ["Infrastructure/Enums/Enums.csproj", "Infrastructure/Enums/"]
COPY ["Infrastructure/Core/Core.csproj", "Infrastructure/Core/"]
COPY ["Infrastructure/ServiceExtensions/ServiceExtensions.csproj", "Infrastructure/ServiceExtensions/"]
COPY ["Infrastructure/Commands/Commands.csproj", "Infrastructure/Commands/"]
COPY ["Infrastructure/Queries/Queries.csproj", "Infrastructure/Queries/"]
RUN dotnet restore "Services/CatalogService/CatalogService.csproj"
COPY . .
WORKDIR "/src/Services/CatalogService"
RUN dotnet build "CatalogService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CatalogService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CatalogService.dll"]