#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Kero-Auth/Kero-Auth.csproj", "Kero-Auth/"]
COPY ["Kero-Auth.Application/Kero-Auth.Application.csproj", "Kero-Auth.Application/"]
COPY ["Kero-Auth.Domain/Kero-Auth.Domain.csproj", "Kero-Auth.Domain/"]
COPY ["Kero-Auth.Infrastructure/Kero-Auth.Infrastructure.csproj", "Kero-Auth.Infrastructure/"]
COPY ["Kero-Auth.Workers/Kero-Auth.Workers.csproj", "Kero-Auth.Workers/"]
RUN dotnet restore "./Kero-Auth/./Kero-Auth.csproj"
COPY . .
WORKDIR "/src/Kero-Auth"
RUN dotnet build "./Kero-Auth.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Kero-Auth.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kero-Auth.dll"]