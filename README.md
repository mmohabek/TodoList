# TodoList API 🚀

![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![Docker](https://img.shields.io/badge/Docker-✔-success)
![Swagger](https://img.shields.io/badge/Swagger-UI-important)
![Architecture](https://img.shields.io/badge/Clean_Architecture-✔-green)

A production-ready task management system with JWT authentication, built on:
- Clean Architecture
- Repository & Unit of Work Patterns
- CQRS with MediatR

## Features ✨

- ✅ Full task management (CRUD operations)
- ✅ JWT Authentication with role-based authorization (Owner/Guest)
- ✅ Advanced filtering & pagination
- ✅ Automated API documentation (Swagger UI)
- ✅ Docker support for containerized deployment
- ✅ Unit & Integration Testing

## Tech Stack 🛠️

| Layer               | Technologies                 |
|---------------------|-----------------------------|
| **API**            | ASP.NET Core 8              |
| **Application**    | MediatR, AutoMapper, FluentValidation |
| **Domain**         | Entity Framework Core 8     |
| **Infrastructure** | Identity, JWT, SQL Server   |
| **Testing**        | xUnit, Moq, TestContainers  |

## Prerequisites 📋

- .NET 8 SDK
- Docker Desktop (for containerization)
- SQL Server (or use Dockerized DB)

## Quick Start 🚀

### Option 1: Run with Docker (Recommended)
```bash
docker-compose up -d
