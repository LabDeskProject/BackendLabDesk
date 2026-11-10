# 🧩 LabDesk Backend

> A modular ASP.NET Core backend for organization-based service requests, approval workflows, assignments, SLA tracking, audit records, and notifications.

## 🎯 Overview

### The problem LabDesk addresses

Organizations often handle internal service requests through disconnected channels such as email, chat, and spreadsheets. This makes it difficult to keep request details in one place, coordinate approvals and assignments, monitor response and resolution targets, and maintain a reliable history of decisions and actions.

LabDesk provides a centralized backend for organization-scoped service requests and their lifecycle. It connects request submission with approval workflows, team/member assignment, SLA tracking, notifications, and audit records, so these activities can be managed and queried through a consistent API.

### What LabDesk is

LabDesk is an ASP.NET Core modular monolith. A single API host composes independently structured business modules while keeping API, Application, Domain, and Infrastructure concerns separated.

The application uses SQL Server for persistent data. Request and approval events are recorded in SQL outbox tables and can be published to RabbitMQ. Prometheus collects application and broker metrics, and Grafana displays those metrics in dashboards.

> **Messaging scope:** RabbitMQ publishing is implemented, but there is not yet a business consumer. Published messages remain in the queue until a consumer is added.

## 📚 Contents

- [Architecture](#architecture)
- [Modules](#modules)
- [Technology Stack](#technology-stack)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Run with Docker Compose](#run-with-docker-compose)
- [Local Development](#local-development)
- [API Documentation](#api-documentation)
- [Authentication](#authentication)
- [Messaging and Outbox](#messaging-and-outbox)
- [Monitoring](#monitoring)
- [Tests](#tests)
- [Configuration and Security](#configuration-and-security)
- [Known Limitations](#known-limitations)

## 🏗️ Architecture

LabDesk uses a modular monolith architecture. The Host project composes the module APIs and registers their dependencies.

Each business module is organized into layers:

- **API** — HTTP controllers and request/response contracts.
- **Application** — commands, queries, handlers, validation, and use-case interfaces.
- **Domain** — entities and business rules.
- **Infrastructure** — SQL Server persistence, repositories, migrations, and external integrations.

```text
Clients
   |
   | HTTP / JWT
   v
ASP.NET Core Host
   |
   +-- IdentityAccess
   +-- OrganizationManagement
   +-- ServiceCatalog
   +-- RequestManagement
   +-- WorkflowApproval
   +-- Assignment
   +-- SLA
   +-- Audit
   +-- Notification
   |
   +-- SQL Server
   |
   +-- SQL Outbox ---> RabbitMQ
   |
   +-- API /metrics --------┐
   +-- RabbitMQ metrics ----+--> Prometheus ---> Grafana
```

## 🧱 Modules

| Module | Responsibility |
|---|---|
| **IdentityAccess** | User registration, login, JWT access tokens, refresh-token rotation, and global administrator management. |
| **OrganizationManagement** | Organizations, memberships, roles, teams, and organization-managed services. |
| **ServiceCatalog** | Organization-scoped catalog services and their lifecycle. |
| **RequestManagement** | Service requests, request numbering, request queries, and request outbox records. |
| **WorkflowApproval** | Approval workflows, approver steps, decisions, and approval outbox records. |
| **Assignment** | Assigning and reassigning requests to organization members and teams. |
| **SLA** | SLA policies, request clocks, response/resolution tracking, and breach queries. |
| **Audit** | Append and query organization/entity audit entries. |
| **Notification** | In-app notifications, delivery processing, and SQL outbox dispatch. |

## 🛠️ Technology Stack

- **.NET 10 / ASP.NET Core** — API host and module projects.
- **Entity Framework Core** — SQL Server persistence and migrations.
- **Dapper** — selected query/read paths.
- **MediatR** — request and command dispatch between API and Application layers.
- **FluentValidation** — input and command validation.
- **JWT Bearer Authentication** — user authentication and authorization.
- **RabbitMQ.Client** — publishing SQL outbox events to RabbitMQ.
- **Docker / Docker Compose** — local multi-container development environment.
- **Prometheus** — metrics scraping and time-series storage.
- **Grafana** — dashboards that query Prometheus.
- **xUnit** — automated tests.

## ✨ Features

### 🔐 Identity and Access

- User registration and login.
- Short-lived JWT access tokens.
- Rotating, single-use refresh tokens.
- Authorization using user roles and organization membership.
- Optional global administrator bootstrap, requiring explicit configuration.

### 🏢 Organization and Service Management

- Create organizations and organization teams.
- Manage organization membership and roles.
- Create organization services and maintain a separate service catalog.

### 📝 Request and Approval Workflows

- Submit service requests scoped to an organization and service.
- Start approval workflows with supported approval modes.
- Record approval decisions and workflow completion events.

### ⏱️ Assignment and SLA

- Assign and reassign requests to organization members and teams.
- Create and manage SLA policies.
- Start, respond to, and resolve SLA clocks.
- Query SLA breaches.

### 🔔 Audit and Notifications

- Append audit entries and query them by organization or entity.
- Generate in-app notifications from handled request/approval outbox events.
- Use idempotency keys to reduce duplicate notification and audit writes during retries.

## 📋 Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) for local development.
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) with Docker Compose for the full local stack.
- Git, if cloning the repository.
- A SQL Server connection if running the API outside Docker Compose.

## 🐳 Run with Docker Compose

### 1. 🔧 Configure local environment variables

Copy the example environment file:

```powershell
Copy-Item .env.example .env
```

Edit `.env` and replace the sample values with unique local credentials and secrets.

> Do not commit `.env` or share its contents. It contains credentials and signing secrets.

### 2. ▶️ Validate and start the stack

From the repository root:

```powershell
docker compose config --quiet
docker compose up -d --build
docker compose ps
```

The migration service waits for SQL Server to become healthy and applies the module migrations. The API waits for the migration service to complete and RabbitMQ to become healthy.

### 3. 🌐 Open the local services

| Service | URL | Notes |
|---|---|---|
| API Swagger UI | <http://localhost:8080/swagger> | Available when the API runs in Development. |
| RabbitMQ Management | <http://localhost:15672> | Sign in with the RabbitMQ credentials from `.env`. |
| Prometheus | <http://localhost:9090> | Use `/targets` to check scrape status. |
| Grafana | <http://localhost:3000> | Sign in with the Grafana credentials from `.env`. |

The API is published on port `8080`. RabbitMQ's AMQP and metrics ports are available inside the Compose network. RabbitMQ Management, Prometheus, and Grafana are bound to loopback for local access.

### 4. ⏹️ Stop the stack

```powershell
docker compose down
```

This preserves named volumes. `docker compose down -v` removes the volumes and their data; use it only when you intentionally want to delete local SQL, RabbitMQ, Prometheus, and Grafana data.

## 💻 Local Development

Restore and build the API host:

```powershell
dotnet restore .\src\Host\Host.csproj
dotnet build .\src\Host\Host.csproj
```

Run the API from the repository root:

```powershell
dotnet run --project .\src\Host\Host.csproj
```

Configure a valid SQL Server connection string and JWT signing key through .NET User Secrets or environment variables before starting the API. Do not put production credentials in source-controlled settings files.

When running the API directly on Windows while SQL Server or RabbitMQ runs in Docker, use a host-accessible address and a published host port. Docker Compose service names such as `sql` and `rabbitmq` resolve only from containers on the Compose network.

## 📖 API Documentation

When running in Development, Swagger UI is available at:

```text
http://localhost:8080/swagger
```

The API includes route groups for:

- Authentication and refresh tokens: `/api/auth`
- Organizations, members, and teams: `/api/organizations`
- Organization services: `/api/services`
- Service catalog: `/api/catalog/services`
- Requests: `/api/requests`
- Approval workflows: `/api/approval-workflows`
- Assignments: `/api/assignments`
- SLA policies and clocks: `/api/sla`
- Notifications: `/api/notifications`
- Audit: `/api/audit`

Most business endpoints require a valid JWT. Use the login endpoint to obtain an access token, then authorize requests in Swagger using the Bearer authentication control.

## 🔑 Authentication

The authentication flow uses user access tokens and refresh tokens:

1. Register a user with `POST /api/auth/register`.
2. Sign in with `POST /api/auth/login`.
3. Use the returned access token for protected endpoints.
4. Rotate a refresh token with `POST /api/auth/tokens/refresh`.
5. Revoke a refresh-token family with `POST /api/auth/tokens/revoke`.

Refresh tokens are sensitive credentials. Do not log or commit them. Use HTTPS in deployed environments.

## 📨 Messaging and Outbox

Request and approval operations write event rows to SQL outbox tables. The background dispatcher claims unprocessed rows, routes the existing notification/audit work idempotently, publishes an event envelope to RabbitMQ, and marks the SQL outbox row processed only after a successful publish confirmation.

RabbitMQ topology includes:

- Durable topic exchange: `labdesk.events`
- Durable quorum queue: `labdesk.events`
- Durable fanout dead-letter exchange: `labdesk.events.dlx`
- Dead-letter queue: `labdesk.events.dead`

Delivery is **at least once**. If RabbitMQ confirms a message but the API stops before updating the SQL outbox row, the dispatcher can publish that event again. A future consumer should deduplicate using the stable event ID and implement safe retry and acknowledgment handling.

> RabbitMQ currently has no business consumer. Published messages remain queued for inspection or future consumer development. Publishing a message does not mean a downstream business operation has completed.

## 📊 Monitoring

Monitoring consists of three cooperating components:

### 🔥 Prometheus — collects and stores metrics

Prometheus periodically requests metrics from configured targets and stores the samples as time-series data. In this local setup, it scrapes:

- The API at `/metrics`.
- RabbitMQ's Prometheus metrics endpoint.
- Prometheus itself.

The scrape interval is 15 seconds. Open <http://localhost:9090/targets> to see whether each target is reachable. A value of `up = 1` means Prometheus successfully scraped that target; it is **not** a complete application readiness check.

Example PromQL queries:

```promql
up
```

```promql
sum(rate(http_requests_received_total[5m]))
```

```promql
histogram_quantile(
  0.95,
  sum by (le) (rate(http_request_duration_seconds_bucket[5m]))
)
```

```promql
sum by (event_type) (increase(labdesk_outbox_published_total[1h]))
```

```promql
sum by (event_type) (increase(labdesk_outbox_publish_failures_total[1h]))
```

### 📈 Grafana — queries Prometheus and visualizes metrics

Grafana uses Prometheus as its data source. The provisioned **LabDesk - API and Messaging Overview** dashboard displays:

- HTTP request rate.
- HTTP request latency at the 95th percentile.
- Confirmed outbox events by event type.
- Outbox publishing failures by event type.
- RabbitMQ messages waiting in queues.
- Prometheus scrape-target health.

Open <http://localhost:3000> and select the provisioned dashboard. Grafana visualizes the measurements; Prometheus is the service that scrapes and stores them.

### 🧪 Generating metrics

Calling API endpoints generates HTTP metrics. Outbox and RabbitMQ message metrics require a business operation that writes an event to the SQL outbox, such as submitting a request or recording an approval decision. There is no need to send meaningless or excessive requests.

The API's `/health/live` and `/health/ready` endpoints currently indicate process-level status; they do not check SQL Server or RabbitMQ connectivity.

## ✅ Tests

Run the solution tests:

```powershell
dotnet test .\LabDeskPrj.slnx
```

The solution contains domain tests for the modules and API contract tests.

## 🛡️ Configuration and Security

- Store SQL connection strings and JWT signing keys in .NET User Secrets, environment variables, or a managed secret store.
- Never commit `.env`, passwords, signing keys, access tokens, or refresh tokens.
- Use unique credentials for local development; rotate credentials if they are exposed.
- Do not expose RabbitMQ AMQP or management ports to untrusted networks.
- The current Compose file sets `ASPNETCORE_ENVIRONMENT=Development` to support local Swagger. Configure Production settings, TLS, network restrictions, backups, and secret management before public deployment.
- Restrict access to `/metrics` to trusted monitoring infrastructure in deployed environments.
- Configure dependency-aware readiness checks before using health endpoints for production orchestration.

## ⚠️ Known Limitations

- RabbitMQ publishing is implemented; there is no business message consumer yet.
- Outbox retries do not currently have a bounded retry schedule or maximum-attempt policy.
- `/health/live` and `/health/ready` are process-level endpoints; readiness does not actively probe SQL Server or RabbitMQ.
- Docker Compose is configured for local development and learning, not as-is production deployment.
- Production database migration/adoption must be planned, backed up, and rehearsed before changes are applied to an existing database.

## 🗺️ Roadmap

- [ ] Add Azure deployment support for the API and background processing.
- [ ] Provision Azure SQL and configure secure application connectivity.
- [ ] Integrate Azure messaging and monitoring services where appropriate.
- [ ] Add deployment automation and production-ready health/readiness checks.

> Azure integration is planned; the current repository does not yet include Azure deployment resources or a completed Azure deployment.

## 🔗 References

- [RabbitMQ .NET client documentation](https://www.rabbitmq.com/client-libraries/dotnet-api-guide)
- [Prometheus configuration](https://prometheus.io/docs/prometheus/latest/configuration/configuration/)
- [Grafana provisioning](https://grafana.com/docs/grafana/latest/administration/provisioning/)
- [Docker Compose documentation](https://docs.docker.com/compose/)

---

**LabDesk Backend**
