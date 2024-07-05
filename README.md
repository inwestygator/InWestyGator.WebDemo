# InWestyGator Web Demo

## Overview

This solution demonstrates a basic ASP.NET Core Web API that manages packs with self-referencing relationships, using Entity Framework Core (EF Core) and PostgreSQL. It also includes a basic authentication handler for demonstration purposes.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop)
- [PostgreSQL](https://www.postgresql.org/download/)

## Getting Started

### Running the Solution with Docker Compose

1. Clone the repository:

    ```bash
    git clone https://github.com/inwestygator/web-demo.git
    cd web-demo
    ```

2. Ensure Docker is running, then start the services:

    ```bash
    docker-compose up
    ```

3. The API should now be accessible at `http://localhost:8080`.

### Running the Solution for Debugging

1. Ensure PostgreSQL is running:

    ```bash
    docker-compose up postgres
    ```
    Or if you wish to run it manually:
    ```bash
    docker run --name postgres -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -v ./data:/var/lib/postgresql/data -d postgres:14.1-alpine
    ```

2. Update the connection string in `appsettings.Development.json` if necessary:

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres"
    }
    ```

3. Start the ASP.NET Core Web API with Kestrel:

    You can select the `http` option for starting the Debug session in Visual Studio. Or from command line:
    ```bash
    dotnet run --project ./InWestyGator.WebDemo
    ```

### API Endpoints

- **GET /api/packs**: Retrieve a pack with its hierarchy.
  - Query Parameters: `id` (string)

- **GET /api/packs/all**: Retrieve all packs with pagination.
  - Query Parameters: `pageNumber` (int, optional), `pageSize` (int, optional)

- **POST /api/packs**: Add a new pack.
  - Body: `Pack` object

### Example JSON for Adding a Pack

```json
{
    "id": "pack.test",
    "packName": "Test Pack",
    "active": true,
    "price": 10,
    "content": [
      "something.nice"
    ]
}
```
    For creating hierarchy one can add additional Pack Id's to the `childPackIds` field:

```json
{
    "id": "pack.test2",
    "packName": "Test Pack 2",
    "active": true,
    "price": 10,
    "content": [
      "something.nicer", "something.nicest"
    ],
    "childPackIds": [
      "pack.test"
    ]
}
```