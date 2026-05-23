# Docker Agent: Containerization Guide

**Purpose**: Guide implementation of Docker/Docker Compose configuration for a multi-container logistics demo.
**Scope**: Oversee Dockerfile(s) and docker-compose.yml for the full-stack app and SQL Server database.

---

## Architecture Vision

**Target State**: Three Docker containers (recommended production pattern)
1. **Backend Container** — Runs .NET 8 ASP.NET Core API
   - Exposes port 5088 (backend API)
   - Connects to SQL Server container via connection string
2. **Frontend Container** — Runs Next.js application
   - Exposes port 3000 (Next.js production server)
   - Connects to backend via `http://backend:5088` (Docker hostname)
3. **SQL Server Container** — Standalone SQL Server 2022 instance
   - Exposes port 1433 (standard SQL Server port)
   - Persists data via Docker volume

**Rationale**: 
- **Separate images**: Each service uses its optimal runtime (ASP.NET for .NET, Node.js for Next.js, SQL Server for DB)
- **Independent scaling**: Deploy frontend without backend, scale one without the other
- **Faster builds**: Modify frontend code, rebuild only frontend container (seconds, not minutes)
- **Cleaner logs**: Each service has its own log stream (`docker-compose logs frontend` vs. `docker-compose logs backend`)
- **Production-ready**: Industry-standard pattern; used by major platforms
- **Flexibility**: Easy to add reverse proxy (nginx), monitoring sidecar, or other services later

---

## Key Design Decisions

### Backend (.NET 8)
- **Multi-stage build** for production: compile in stage 1, copy built artifacts to minimal stage 2 (reduces final image size).
- **Runtime base image**: `mcr.microsoft.com/dotnet/aspnet:8.0` (official Microsoft image, kept updated).
- **Build base image**: `mcr.microsoft.com/dotnet/sdk:8.0` (includes build tools).
- **Connection string override**: App must read DB connection from environment variable `SQLSERVER_CONNECTION` (injected via `docker-compose.yml`).
  - Current `Program.cs` reads from `appsettings.json`; when Dockerized, override via `appsettings.Docker.json` or env var substitution.
- **Database migrations**: Run `dotnet ef database update` on startup (handled in an init script or via EF Core in code).
- **HTTP port**: 5088 (matches launchSettings.json); HTTPS optional in container (reverse proxy handles it).

### Frontend (Next.js)
- **Build stage**: Run `npm install` and `npm run build` (compiles static assets and server functions).
- **Runtime**: Node.js 20 Alpine (lightweight base image for production).
- **Multi-stage build**: Stage 1 builds app (with dev dependencies), Stage 2 runs only production dependencies.
- **Environment variable**: `NEXT_PUBLIC_API_BASE_URL` must point to backend container hostname: `http://backend:5088` (internally), or `http://localhost:5088` from outside container.
- **Port**: 3000 (Next.js default).
- **Health check**: Uses `wget` to verify service is up.

### SQL Server
- **Base image**: `mcr.microsoft.com/mssql/server:2022-latest` (official Microsoft SQL Server image).
- **Port**: 1433 (internal) → mapped to host (development) or private to app network (production).
- **Volume**: Mount `/var/opt/mssql` to persist database files across restarts.
- **Environment variables**:
  - `ACCEPT_EULA=Y` (required; user accepts SQL Server license)
  - `SA_PASSWORD` (system admin password; minimum 8 chars, uppercase/lowercase/digits/symbols)
- **Health check**: Use `sqlcmd` to verify readiness before app starts.

---

## File Structure

```
logistics-visualization-demo/
├── Dockerfile                      # Multi-stage build for .NET 8 backend
├── docker-compose.yml              # Orchestrate backend + frontend + SQL Server
├── .dockerignore                   # Exclude unnecessary files from Docker context
├── Agents/
│   └── docker-agent.md             # This file
├── Program.cs                       # Updated to load appsettings.Docker.json
├── appsettings.Docker.json         # Docker-specific DB connection config
├── client/
│   ├── Dockerfile                  # Multi-stage build for Next.js frontend
│   ├── .dockerignore               # Exclude node_modules, .next, .git
│   ├── package.json
│   └── app/
└── ...existing files
```

---

## Dockerfile Templates

### Backend Dockerfile (project root)

```dockerfile
# Multi-stage Dockerfile for .NET 8 Backend
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
```

### Frontend Dockerfile (client/Dockerfile)

```dockerfile
# Multi-stage Dockerfile for Next.js Frontend

# Stage 1: Build Next.js app
FROM node:20-alpine AS builder
WORKDIR /app

# Install dependencies
COPY package*.json ./
RUN npm ci

# Build Next.js app
COPY . .
RUN npm run build

# Stage 2: Production runtime
FROM node:20-alpine AS runtime
WORKDIR /app

# Copy package files
COPY package*.json ./

# Install production dependencies only
RUN npm ci --only=production

# Copy built app from builder
COPY --from=builder /app/.next ./.next
COPY --from=builder /app/public ./public

# Expose frontend port
EXPOSE 3000

# Health check
HEALTHCHECK --interval=10s --timeout=3s --start-period=30s --retries=3 \
    CMD wget --quiet --tries=1 --spider http://localhost:3000 || exit 1

# Start Next.js app in production mode
CMD ["npm", "start"]
```

---

## docker-compose.yml Template

```yaml
version: '3.9'

services:
  # SQL Server 2022 Database
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: logistics-sqlserver
    environment:
      ACCEPT_EULA: "Y"
      SA_PASSWORD: "YourUpdatedPassword123!"  # CHANGE: Use .env file
    ports:
      - "1433:1433"
    volumes:
      - sqlserver-data:/var/opt/mssql
    healthcheck:
      test: 
        - CMD
        - /opt/mssql-tools18/bin/sqlcmd
        - -S
        - localhost
        - -U
        - sa
        - -P
        - "YourUpdatedPassword123!"
        - -Q
        - "SELECT 1"
      interval: 10s
      timeout: 5s
      retries: 5
      start_period: 10s
    networks:
      - app-network
    restart: unless-stopped

  # .NET 8 Backend API
  backend:
    build:
      context: .
      dockerfile: Dockerfile
    container_name: logistics-backend
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - SQLSERVER_CONNECTION=Server=sqlserver,1433;Database=RecordContext;User Id=sa;Password=YourUpdatedPassword123!;Encrypt=no;
    ports:
      - "5088:5088"
    depends_on:
      sqlserver:
        condition: service_healthy
    networks:
      - app-network
    restart: unless-stopped

  # Next.js Frontend
  frontend:
    build:
      context: ./client
      dockerfile: Dockerfile
    container_name: logistics-frontend
    environment:
      - NEXT_PUBLIC_API_BASE_URL=http://backend:5088
    ports:
      - "3000:3000"
    depends_on:
      - backend
    networks:
      - app-network
    restart: unless-stopped

volumes:
  sqlserver-data:
    driver: local

networks:
  app-network:
    driver: bridge
```

**Key Points**:
- **backend**: Internal hostname resolves to backend service (available as `http://backend:5088` from frontend container)
- **frontend**: Uses `http://backend:5088` internally; external access via `http://localhost:3000`
- **depends_on**: Frontend waits for backend; backend waits for healthy SQL Server
- **restart**: All services restart automatically unless manually stopped

---

## .dockerignore Templates

### Root .dockerignore (for backend)

```
node_modules
npm-debug.log
.git
.gitignore
.env
.env.local
.env.*.local
*.md
!README.md
.code-workspace

# .NET build artifacts
bin/
obj/
*.dll
*.pdb
.vs/
.vscode/

# Frontend
client/node_modules
client/.next
client/dist
client/.env.local

# Tests
**/*.Tests/bin
**/*.Tests/obj

# IDE & Editor
.idea/
*.code-workspace
Thumbs.db
.DS_Store

# Docker & deployment
docker-compose.override.yml
Dockerfile
.dockerignore
```

### client/.dockerignore (for frontend)

```
node_modules
npm-debug.log
.git
.gitignore
.env.local
.env.*.local
*.md
.code-workspace

# Next.js
.next
dist
.env

# IDE
.idea/
.vscode/
Thumbs.db
.DS_Store
```

---

## Implementation Checklist

- [ ] Create `Dockerfile` at root (multi-stage .NET 8 build for backend)
- [ ] Create `client/Dockerfile` for Next.js frontend
- [ ] Create `docker-compose.yml` orchestrating backend + frontend + SQL Server
- [ ] Create `.dockerignore` (root) and `client/.dockerignore` to reduce build context
- [ ] Create `appsettings.Docker.json` with container-specific connection string
- [ ] Update `Program.cs` to conditionally load `appsettings.Docker.json` when running in Docker
- [ ] Test locally: `docker-compose up --build`
- [ ] Verify all containers are running: `docker-compose ps`
- [ ] Verify backend API is reachable: `curl http://localhost:5088/api/data/orders`
- [ ] Verify frontend loads: `http://localhost:3000`
- [ ] Check logs for errors: `docker-compose logs -f`
- [ ] Verify database seeding works (orders table should have rows)
- [ ] Document in main `README.md`: "To run with Docker: `docker-compose up --build`"

---

## Common Pitfalls & Solutions

| Pitfall | Cause | Fix |
|---------|-------|-----|
| Container exits immediately | DB not reachable, wrong connection string | Add `depends_on: { sqlserver: { condition: service_healthy } }` and verify SA_PASSWORD matches in compose and app config |
| Frontend can't reach backend | API_BASE_URL points to localhost:5088 (not accessible from container) | Use container hostname: `http://app:5088` internally; map port 5088 on host for external access |
| SQL Server won't start | SA_PASSWORD too weak or not set | Ensure SA_PASSWORD has uppercase, lowercase, digits, symbols; min 8 chars |
| Data lost on container restart | No volume mount | Add `volumes: - sqlserver-data:/var/opt/mssql` to docker-compose.yml |
| Slow build | Unnecessary files in Docker context | Add `.dockerignore` entries (node_modules, bin/, obj/, .git) |
| Migrations don't run | EF Core not invoked on app startup | Call `context.Database.Migrate()` in `Program.cs` or wrap in an init script |

---

## Performance & Production Considerations

1. **Image size**: Use Alpine Linux for Node.js, multi-stage builds for .NET, exclude dev dependencies.
2. **Caching layers**: Order Dockerfile commands (stable → frequently-changing) to maximize layer reuse.
3. **Secrets**: Never hardcode SA_PASSWORD in Dockerfile or compose; use `.env` or Docker Secrets (Swarm/K8s).
4. **Logging**: Redirect app logs to stdout/stderr (Docker will capture them); use `docker logs <container>`.
5. **Resource limits**: Set memory/CPU limits in docker-compose:
   ```yaml
   deploy:
     resources:
       limits:
         cpus: '1'
         memory: 2G
   ```
6. **Restart policy**: Add `restart: unless-stopped` to services for durability.

---

## Testing & Debugging

**Build images**:
```bash
docker-compose build
```

**Start containers**:
```bash
docker-compose up
```

**View logs**:
```bash
docker-compose logs -f app
docker-compose logs -f sqlserver
```

**Execute command in running container**:
```bash
docker-compose exec app dotnet ef database update
docker-compose exec sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P <PASSWORD> -Q "SELECT 1"
```

**Tear down**:
```bash
docker-compose down --volumes  # Remove volumes if you want fresh DB
docker-compose down             # Keep volumes
```

---

## References & Links

- [Microsoft .NET Docker Best Practices](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/container-docker-introduction/docker-application-state-data)
- [Next.js Docker Setup](https://nextjs.org/docs/deployment/docker)
- [SQL Server Docker Docs](https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker)
- [Docker Compose Networking](https://docs.docker.com/compose/networking/)
- [Multi-stage Builds](https://docs.docker.com/build/building/multi-stage/)

---

## Next Steps for AI Agents

When implementing Docker files:
1. Read this agent guide **first** to understand the architecture vision.
2. Follow the **Implementation Checklist** in order.
3. Test each step locally (`docker-compose up --build`).
4. If you encounter errors, consult **Common Pitfalls & Solutions**.
5. Update this guide if you discover new patterns or gotchas specific to this codebase.

---

**Last Updated**: April 1, 2026  
**Owner**: Docker Agent  
**Related Files**: `Dockerfile`, `docker-compose.yml`, `Program.cs`, `appsettings.Docker.json`
