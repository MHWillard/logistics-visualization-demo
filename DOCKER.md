# Docker Deployment Guide

This guide covers running the Logistics Visualization Demo in Docker containers.

## Quick Start

### Prerequisites
- Docker Desktop installed and running (Windows: WSL 2 backend recommended)
- Or Docker + Docker Compose CLI tools

### 1. Build and Start Containers

From the repository root:

```bash
docker-compose up --build
```

This will:
1. Build the .NET 8 backend image
2. Build the Next.js frontend image
3. Start SQL Server container (on port 1433)
4. Start backend container (on port 5088)
5. Start frontend container (on port 3000)
6. Run database migrations and seed data

### 2. Verify Services

```bash
# Check all containers are running
docker-compose ps

# View logs from all containers
docker-compose logs -f

# View logs from specific service
docker-compose logs -f backend
docker-compose logs -f frontend
docker-compose logs -f sqlserver

# Test backend API
curl http://localhost:5088/api/data/orders

# Test frontend
# Open http://localhost:3000 in browser
```

### 3. Stop Containers

```bash
# Stop and remove containers
docker-compose down

# Stop and remove containers + volumes (clears database)
docker-compose down --volumes
```

---

## Configuration

### Environment Variables

Create a `.env` file from `.env.example`:

```bash
cp .env.example .env
```

Edit `.env` and update sensitive values:
```
SA_PASSWORD=YourUpdatedPassword123!
ASPNETCORE_ENVIRONMENT=Production
NEXT_PUBLIC_API_BASE_URL=http://localhost:5088
```

**Important**: Never commit `.env` to version control; use `.env.example` as a template only.

### Connection String

The backend uses the connection string from `appsettings.Docker.json`:

```
Server=sqlserver,1433;Database=RecordContext-0e9;User Id=sa;Password=YourUpdatedPassword123!;...
```

- `sqlserver` = Docker container hostname (resolved via Docker DNS)
- `1433` = SQL Server default port
- `TrustServerCertificate=true` = Disable SSL for local dev (add encryption for production)

---

## Common Tasks

### Run Database Migrations

```bash
docker-compose exec backend dotnet ef database update
```

### Access SQL Server CLI

```bash
docker-compose exec sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourUpdatedPassword123! -Q "SELECT 1"
```

### View Application Logs

```bash
docker-compose logs -f app
```

### Rebuild Images (after code changes)

```bash
docker-compose build --no-cache
docker-compose up
```

### Remove Everything and Start Fresh

```bash
docker-compose down --volumes
docker-compose up --build
```

---

## Architecture

**Three-Container Setup** (recommended):
- **Backend Container**: .NET 8 ASP.NET Core API on port 5088
- **Frontend Container**: Next.js on port 3000
- **SQL Server Container**: SQL Server 2022 on port 1433 (internal) with persistent volume

**Communication**:
- Frontend connects to backend via hostname `http://backend:5088` (Docker network DNS)
- Backend connects to SQL Server via hostname `sqlserver:1433`
- Both containers on `app-network` bridge network
- Volumes ensure database persists across container restarts

---

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Container exits immediately | Check logs: `docker-compose logs app`. Verify connection string and SA_PASSWORD match. |
| Cannot connect to SQL Server | Ensure `sqlserver` container is running: `docker-compose ps`. Check health: `docker-compose ps sqlserver`. |
| Port already in use | Change ports in `docker-compose.yml` or stop other services: `docker-compose down`. |
| Database not initializing | Verify `appsettings.Docker.json` connection string. Check migrations in `Migrations/` folder. |
| Slow performance on Windows | Use WSL 2 backend for Docker Desktop. Enable resource allocation in Docker settings. |

---

## Development vs. Production

### Development (Local)
```bash
docker-compose up  # Uses default settings, debug logging enabled
```

### Production
- Use `docker-compose.override.yml` for environment-specific overrides
- Set strong `SA_PASSWORD`
- Add resource limits and restart policies
- Use external database or managed SQL Server
- Disable Swagger UI
- Enable HTTPS with proper certificates

---

## For More Details

See `Agents/docker-agent.md` for comprehensive Docker architecture, multi-stage build details, performance tuning, and best practices.
