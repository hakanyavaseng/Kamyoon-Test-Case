FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProductManagement.WebAPI/ProductManagement.WebAPI.csproj", "ProductManagement.WebAPI/"]
COPY ["ProductManagement.Persistence/ProductManagement.Persistence.csproj", "ProductManagement.Persistence/"]
COPY ["ProductManagement.Application/ProductManagement.Application.csproj", "ProductManagement.Application/"]
COPY ["ProductManagement.Domain/ProductManagement.Domain.csproj", "ProductManagement.Domain/"]
COPY ["ProductManagement.Infrastructure/ProductManagement.Infrastructure.csproj", "ProductManagement.Infrastructure/"]
RUN dotnet restore "ProductManagement.WebAPI/ProductManagement.WebAPI.csproj"
COPY . .
WORKDIR "/src/ProductManagement.WebAPI"
RUN dotnet build "ProductManagement.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProductManagement.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductManagement.WebAPI.dll"]
