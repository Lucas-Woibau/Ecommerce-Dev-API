# EcommerceDev

Professional e-commerce reference application built with modern .NET.

## Table of contents

- [Overview](#overview)
- [Features](#features)
- [Technology stack](#technology-stack)
- [Repository layout](#repository-layout)
- [Prerequisites](#prerequisites)
- [Local setup](#local-setup)
- [Configuration and secrets](#configuration-and-secrets)
- [Database migrations](#database-migrations)
- [Running the application](#running-the-application)
- [Testing](#testing)
- [Deployment](#deployment)
- [Security & best practices](#security--best-practices)
- [Contributing](#contributing)
- [License](#license)

## Overview

This repository contains the backend and supporting infrastructure for an e-commerce application. It demonstrates practical, production-oriented patterns for building a cloud-ready .NET application: separation of concerns, environment-based configuration, secure secrets handling, and integrations with external services (Postgres, Azure Storage, Stripe, Geolocation APIs).

## Features

- Product and catalog management
- Shopping cart and checkout flow
- Payment processing (Stripe integration)
- File/blob storage (Azure Storage)
- Geolocation / address lookup integration
- Database persistence with EF Core (recommended)
- Environment-aware configuration and user secrets

## Technology stack

- .NET 10
- C#
- PostgreSQL
- Azure Blob Storage
- Stripe (payments)
- EF Core (ORM and migrations)

## Repository layout

Project layout may vary. Typical projects you will find:

- `EcommerceDev.Api` — Web API and entry point
- `EcommerceDev.Core` — Domain models, DTOs, interfaces
- `EcommerceDev.Infrastructure` — Data, storage, payment integrations
- `tests/` — Unit and integration tests

Open the solution in Visual Studio or run `dotnet sln list` to inspect project names.

## Prerequisites

- .NET 10 SDK
- PostgreSQL instance
- Azure Storage account (or compatible blob storage)
- Stripe account (test keys for development)
- (Optional) `dotnet-ef` global tool for migrations

## Local setup

1. Clone the repository:

   ```powershell
   git clone https://github.com/Lucas-Woibau/Ecommerce-Dev.git
   cd EcommerceDev
   ```

2. Restore and build:

   ```powershell
   dotnet restore
   dotnet build
   ```

## Configuration and secrets

This project uses environment variables or .NET user-secrets for sensitive configuration. Do not commit secret values to source control.

Required configuration keys (names may vary slightly by project):

- `ConnectionStrings:ECommerceDevDb` — Postgres connection string
- `StorageAccount` or `AzureStorage:ConnectionString` — Azure Storage
- `Stripe:ApiKey` — Stripe secret key
- `Geolocation:GeolocationApiKey` — Geolocation API key

Example using user-secrets (run from the project that holds the secrets configuration):

```powershell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:ECommerceDevDb" "Host=localhost;Port=5432;Database=ecommerce_db;Username=postgres;Password=YOUR_PASSWORD"
dotnet user-secrets set "Stripe:ApiKey" "sk_test_..."
dotnet user-secrets set "Geolocation:GeolocationApiKey" "YOUR_GEO_API_KEY"
dotnet user-secrets set "StorageAccount" "DefaultEndpointsProtocol=...;AccountName=...;AccountKey=..."
```

Or set environment variables in your shell / CI environment (for example `Stripe__ApiKey` for ASP.NET Core hierarchical binding).

## Database migrations

If the solution uses EF Core, apply migrations locally:

```powershell
dotnet tool install --global dotnet-ef
dotnet ef database update --project <ProjectContainingMigrations> --startup-project <StartupProject>
```

Replace the `--project` and `--startup-project` values with the actual project names in this solution.

## Running the application

Run the startup project (API) from Visual Studio or with the CLI:

```powershell
dotnet run --project <StartupProject>
```

By default the app may be available at `https://localhost:5001` or as configured in launch settings.

## Testing

Run all tests with:

```powershell
dotnet test
```

Run a single test project if you prefer to scope execution.

## Deployment

- Containerize with a `Dockerfile` if present and push to your container registry.
- Use a cloud secret manager (Azure Key Vault, AWS Secrets Manager) for production secrets.
- Use a managed Postgres service in production and restrict credentials to least privilege.

## Security & best practices

- Never commit secrets to the repository. Use user-secrets locally and a secret store in production.
- Use Stripe test keys during development and rotate keys regularly.
- Serve all endpoints over HTTPS and secure webhook endpoints.
- Apply least-privilege principles to database and storage credentials.

## Contributing

Contributions are welcome.