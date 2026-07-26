# FadTask - Task Management API

A .NET 9 Web API for managing tasks with JWT authentication.

## Tech Stack

- **Runtime**: .NET 9
- **Auth**: JWT Bearer tokens
- **Persistence**: In-memory collections (singleton repositories)
- **Packages**: OpenApi, JWT Bearer, DotNetEnv

## Project Structure

```
Controllers/
  AuthController.cs      # POST api/auth/login
  TasksController.cs     # GET|POST api/tasks
Services/
  AuthService.cs         # JWT token generation
  TaskService.cs         # Task business logic
Repositories/
  UserRepository.cs      # In-memory user store
  TaskRepository.cs      # In-memory task store
Models/
  User.cs                # Id, Email, Password, CreatedAt
  TaskItem.cs            # Id, Title, Description, IsCompleted, CreatedAt
Dtos/
  LoginRequest.cs        # Email, Password (validated)
  TaskCreateRequest.cs   # Title (required), Description (optional)
  TaskResponse.cs        # Task output DTO
Program.cs               # App entry point & DI setup
```

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Setup

1. Clone the repo and navigate to the project directory.

2. Create a `.env` file (copy from `.env.example`):

```bash
cp .env.example .env
```

3. Update `.env` with your values:

```env
Jwt__Key=your-256-bit-secret-key-here
Jwt__ExpireMinutes=60
DEMO_EMAIL=admin@example.com
DEMO_PASSWORD=password
```

4. Run the application:

```bash
dotnet run
```

The API starts on the URL configured in `Properties/launchSettings.json` (default: `http://localhost:5183`).

### OpenAPI / Swagger

In development mode, OpenAPI endpoints are available at `/openapi/v1.json`.

## API Endpoints

### Authentication

| Method | Path           | Description          |
| ------ | -------------- | -------------------- |
| POST   | `api/auth/login` | Authenticate & get JWT |

**Request body:**
```json
{
  "email": "admin@example.com",
  "password": "password"
}
```

**Response (200):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

### Tasks (all endpoints require `Authorization: Bearer <token>`)

| Method | Path        | Description       |
| ------ | ----------- | ----------------- |
| GET    | `api/tasks` | List all tasks    |
| POST   | `api/tasks` | Create a new task |

**POST request body:**
```json
{
  "title": "My Task",
  "description": "Optional description"
}
```

## Configuration

All configuration is via environment variables (loaded from `.env`):

| Variable           | Description               | Default |
| ------------------ | ------------------------- | ------- |
| `Jwt__Key`         | JWT signing key (required) | —       |
| `Jwt__ExpireMinutes` | Token expiry in minutes  | `60`    |
| `DEMO_EMAIL`       | Demo user email           | —       |
| `DEMO_PASSWORD`    | Demo user password        | —       |

> **Note:** Passwords are stored in plaintext — this is a demo application only.

## Example Usage

```bash
# Login
TOKEN=$(curl -s -X POST http://localhost:5183/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"password"}' \
  | jq -r '.token')

# List tasks
curl -s http://localhost:5183/api/tasks \
  -H "Authorization: Bearer $TOKEN" | jq

# Create task
curl -s -X POST http://localhost:5183/api/tasks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"title":"Buy groceries","description":"Milk, eggs, bread"}' | jq
```
