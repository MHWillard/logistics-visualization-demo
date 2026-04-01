# Multi-stage Dockerfile for .NET 8 Backend
# See Agents/docker-agent.md for comprehensive guidance
# Frontend has its own Dockerfile in client/

# Stage 1: Build .NET 8 Backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["logistics-visualization-demo.csproj", "./"]
RUN dotnet restore "logistics-visualization-demo.csproj"

COPY . .
RUN dotnet build "logistics-visualization-demo.csproj" -c Release -o /app/build

# Stage 2: Publish Backend
FROM build AS publish
RUN dotnet publish "logistics-visualization-demo.csproj" -c Release -o /app/publish

# Stage 3: Runtime - ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=publish /app/publish .

# Health check for backend API
HEALTHCHECK --interval=10s --timeout=3s --start-period=40s --retries=3 \
    CMD dotnet /app/logistics-visualization-demo.dll || exit 1

# Expose backend API port
EXPOSE 5088

ENTRYPOINT ["dotnet", "logistics-visualization-demo.dll"]
