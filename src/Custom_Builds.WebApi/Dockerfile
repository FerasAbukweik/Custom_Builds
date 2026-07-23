FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Custom_Builds.WebApi/Custom_Builds.WebApi.csproj", "src/Custom_Builds.WebApi/"]
COPY ["src/Custom_Builds.Infrastructure/Custom_Builds.Infrastructure.csproj", "src/Custom_Builds.Infrastructure/"]
COPY ["src/Custom_Builds.Core/Custom_Builds.Core.csproj", "src/Custom_Builds.Core/"]
RUN dotnet restore "src/Custom_Builds.WebApi/Custom_Builds.WebApi.csproj"
COPY . .
WORKDIR /src/src/Custom_Builds.WebApi
RUN dotnet build "./Custom_Builds.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Custom_Builds.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Custom_Builds.WebApi.dll"]
