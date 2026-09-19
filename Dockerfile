# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["IMS/IMS.csproj", "IMS/"]
COPY ["IMS_DomainLayer/IMS_DomainLayer.csproj", "IMS_DomainLayer/"]
COPY ["IMS_RepositoryLayer/IMS_RepositoryLayer.csproj", "IMS_RepositoryLayer/"]
COPY ["IMS_ServiceLayer/IMS_ServiceLayer.csproj", "IMS_ServiceLayer/"]
COPY ["IMS_InfrastructureLayer/IMS_InfrastructureLayer.csproj", "IMS_InfrastructureLayer/"]

RUN dotnet restore "IMS/IMS.csproj"

COPY . .
RUN dotnet publish "IMS/IMS.csproj" -c Release -o /app/publish /p:UseAppHost=false


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "IMS.dll"]