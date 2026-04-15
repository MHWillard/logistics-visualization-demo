<!--
Concise, actionable instructions for AI coding agents working in this repository.
Keep this file small (20–50 lines) and reference concrete files/commands.
-->

# Quick context for AI assistants

This repo is a small full-stack demo: a .NET 8 Web API backend and a Next.js (App Router) frontend in `client/`.

Key boundaries
- Backend: top-level C# project (Program.cs, Controllers/, Data/, Models/). API base routes are `api/data/*` implemented in `Controllers/DataController.cs`.
- Frontend: `client/` is a Next.js app (App Router). Client fetches backend via `client/lib/api.ts` using env var `NEXT_PUBLIC_API_BASE_URL`.

Important files to read first
- `Program.cs` — shows DI, EF Core setup, CORS policy `AllowFrontend`, and automatic DB seed via `DbInitializer`.
- `Controllers/DataController.cs` — all HTTP endpoints. Note: endpoints return serialized JSON strings (not IActionResult).
- `Data/RecordContext.cs` — DbSets and EF model configuration (MonthlyOrderStats is mapped to a DB view with HasNoKey()).
- `Data/DbInitializer.cs` — initial seed data (orders, companies, products, order details).
- `client/lib/api.ts` and `client/app/page.tsx` — how the frontend calls the backend and consumes data.

Developer workflows (commands you can run)
- Run backend dev server (project root):
  - dotnet run (uses `Properties/launchSettings.json` ports; HTTP profile listens on http://localhost:5088)
  - dotnet build
  - dotnet test ./logistics-visualization-demo.Tests
- Run frontend (from `client/`):
  - npm install
  - npm run dev (Next.js dev server at http://localhost:3000)
- Run with Docker (project root):
  - docker-compose up --build (starts app + SQL Server containers; see `DOCKER.md` and `Agents/docker-agent.md`)
  - docker-compose down
  - docker-compose logs -f app (tail logs)

Runtime notes & environment
- Default backend DB: LocalDB (connection string in `appsettings.json` under `RecordContext`). For local development this is fine; CI or containers may need a different SQL Server and updated CONNECTION string.
- Frontend expects `NEXT_PUBLIC_API_BASE_URL` to point to the backend (e.g. http://localhost:5088). Set it in `client/.env.local` during dev.
- CORS: `Program.cs` allows origin `http://localhost:3000` via policy `AllowFrontend`.

Project-specific patterns and gotchas
- Controller methods serialize with `JsonSerializer.Serialize(...)` and return string. This means status codes and model-binding behavior are minimal—be careful when refactoring to IActionResult/ActionResult<T>.
- `MonthlyOrderStats` is a read-only EF Core entity mapped to a database view (`ToView(...).HasNoKey()`). Treat it as projection-only.
- DB seeding is performed on startup by `DbInitializer.Initialize(context)` after EnsureCreated(). Migrations exist under `Migrations/` if you need schema evolution.
- Frontend uses the App Router and React client components (`'use client'`) and Chart.js (`react-chartjs-2`). Styling uses Tailwind utility classes.

Where tests live
- `logistics-visualization-demo.Tests/ControllerTests.cs` — unit/integration-style tests for controllers. Use `dotnet test` to run.

Docker & Containerization
- See `Agents/docker-agent.md` for comprehensive Docker architecture and best practices.
- Key files: `Dockerfile` (multi-stage build), `docker-compose.yml` (two-container orchestration), `appsettings.Docker.json` (container config).
- Quick start: `docker-compose up --build` from project root.
- Read `DOCKER.md` for troubleshooting and common tasks.

Examples to mimic
- To call the monthly stats endpoint: GET `${API_BASE}/api/data/monthly` (see `client/lib/api.ts`).
- If adding a new API route, update `Controllers/DataController.cs`, add EF DbSet in `Data/RecordContext.cs` if it maps to a table, and update the frontend fetch helper in `client/lib/api.ts`.

If anything is ambiguous, ask for the desired goal (fix, feature, test) and which side (backend/frontend) to change.

---
Please review: any missing files or workflows you'd like the assistant to include?
