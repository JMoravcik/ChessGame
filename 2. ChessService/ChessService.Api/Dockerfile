# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["2. ChessService/ChessService.Api/ChessService.Api.csproj", "2. ChessService/ChessService.Api/"]
COPY ["1. Common/ChessGame.Backend.Common/ChessGame.Backend.Common.csproj", "1. Common/ChessGame.Backend.Common/"]
COPY ["1. Common/ChessGame.Common/ChessGame.Common.csproj", "1. Common/ChessGame.Common/"]
COPY ["2. ChessService/ChessService.ChessLogic/ChessService.ChessLogic.csproj", "2. ChessService/ChessService.ChessLogic/"]
COPY ["2. ChessService/ChessService.Contracts/ChessService.Contracts.csproj", "2. ChessService/ChessService.Contracts/"]
COPY ["3. AccessService/AccessService.Contracts/AccessService.Contracts.csproj", "3. AccessService/AccessService.Contracts/"]
RUN dotnet restore "./2. ChessService/ChessService.Api/ChessService.Api.csproj"
COPY . .
WORKDIR "/src/2. ChessService/ChessService.Api"
RUN dotnet build "./ChessService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ChessService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChessGame.ChessService.Api.dll"]