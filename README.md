# Programming II, Technology II, Systems Development II pilot program 2024

- "Contract based" API Development
- REPR pattern
- No persistence abstraction
- Feature-per-endpoint (iteration of vertical slicing)

## Data schema:

Can be found in directory `server-app/api/Boilerplate/DbHelpers/DbScripts.cs`

## Quickstart with Docker Compose:

```bash
# coming soon
```

## Quickstart IntelliJ Dev Container:

```bash
# coming soon
```

## Quickstart "manually" with .NET CLI + vite

*(if you want to start the server app with another DB than local container, simply set an env variable PG_CONN with
postgres connection string and set env variable SKIP_DB_CONTAINER_BUILDING to true)*

Start API on port 5000 (+ starts postgres DB in container on port 5432):

```bash
cd server-app/api && dotnet run
```

Start client:

```bash
cd client-app && npm run dev
```

