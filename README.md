# 🚀 Get Me My Order

> A modern, real-time order management system built with .NET 9, Next.js 15, and SignalR

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Next.js](https://img.shields.io/badge/Next.js-15-000000?logo=next.js)](https://nextjs.org/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5-3178C6?logo=typescript)](https://www.typescriptlang.org/)
[![SignalR](https://img.shields.io/badge/SignalR-WebSocket-0078D4)](https://dotnet.microsoft.com/apps/aspnet/signalr)

## ✨ Features

### 🎯 Core Functionality

- ✅ **Customer Management** - Create and manage customer profiles
- ✅ **Product Catalog** - Maintain product inventory with pricing
- ✅ **Order Processing** - Create and track orders through their lifecycle
- ✅ **Real-time Updates** - Live order status updates without page refresh
- ✅ **Status Tracking** - Monitor orders: Pending → Processing → Done

### 🔥 Technical Highlights

- ✅ **Real-time WebSocket** - SignalR for instant updates
- ✅ **Clean Architecture** - Domain-driven design with CQRS pattern
- ✅ **Microservices** - Separated API and Worker services
- ✅ **Message Queue** - Azure Service Bus for async processing
- ✅ **Modern UI** - Tailwind CSS with responsive design
- ✅ **Type Safety** - Full TypeScript implementation
- ✅ **Health Checks** - Built-in monitoring endpoints

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                         Frontend                                  │
│            Next.js 15 + TypeScript + Tailwind                     │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐        │
│  │ Order List    │  │ Order Detail   │  │ Create Order  │        │
│  │ (Real-time)   │  │ (Real-time)    │  │               │        │
│  └──────────────┘  └──────────────┘  └──────────────┘        │
└─────────────────────────────────────────────────────────────┘
           │                          │                        │
           │ HTTP REST                │ WebSocket              │
           ▼                          ▼                        │
┌─────────────────────┐    ┌─────────────────────┐         │
│   API Service         │    │   Worker Service      │         │
│   (.NET 9)            │    │   (.NET 9)            │         │
│   Port 5000           │    │   Port 5001           │         │
│                       │    │                       │         │
│  ┌──────────────┐   │    │  ┌──────────────┐    │         │
│  │ Controllers   │   │    │  │ SignalR Hub    │    │◄───────┘
│  └──────────────┘   │    │  └──────────────┘    │
│  ┌──────────────┐   │    │  ┌──────────────┐    │
│  │  Use Cases    │   │    │  │  Consumers     │    │
│  └──────────────┘   │    │  └──────────────┘    │
└─────────────────────┘    └─────────────────────┘
           │                          │
           │                          │
           ▼                          ▼
┌─────────────────────┐    ┌─────────────────────┐
│  PostgreSQL           │    │ Azure Service Bus     │
│  (Database)           │    │ (Message Queue)       │
└─────────────────────┘    └─────────────────────┘
```

## 📦 Tech Stack

### Backend

- 🔷 **.NET 9** - Modern C# framework
- 🗄️ **Entity Framework Core** - ORM and database management
- 🚌 **Azure Service Bus** - Async message processing
- 📡 **SignalR** - Real-time WebSocket communication
- 🐘 **PostgreSQL** - Relational database
- ✅ **Health Checks UI** - Service monitoring

### Frontend

- ⚛️ **Next.js 15** - React framework with SSR
- 📘 **TypeScript** - Type-safe JavaScript
- 🎨 **Tailwind CSS** - Utility-first CSS framework
- 📡 **SignalR Client** - WebSocket client library
- 🎯 **Radix UI** - Accessible component primitives

## 🚀 Getting Started

### Prerequisites

Make sure you have the following installed:

- [ ] [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [ ] [Node.js 18+](https://nodejs.org/)
- [ ] [PostgreSQL 15+](https://www.postgresql.org/download/)
- [ ] [Docker](https://www.docker.com/) (optional, for containers)

### 📋 Quick Start Checklist

#### 1️⃣ Clone the Repository

```bash
git clone https://github.com/felipessac/get-me-my-order.git
cd get-me-my-order
```

#### 2️⃣ Setup Backend

```bash
cd backend

# Create environment file
cp .env.example .env

# Update .env with your configuration:
# - PostgreSQL connection string
# - Azure Service Bus connection string

# Run database migrations
dotnet ef database update --project src/Module/Order

# Start the API service
cd src/Shared/Infrastructure/Api
dotnet run
# ✅ API running at http://localhost:5000

# In a new terminal, start the Worker service
cd src/Shared/Infrastructure/Worker
dotnet run
# ✅ Worker running at http://localhost:5001
```

#### 3️⃣ Setup Frontend

```bash
cd frontend

# Install dependencies
npm install

# Create environment file
cp .env.example .env.local

# Start development server
npm run dev
# ✅ Frontend running at http://localhost:3000
```

#### 4️⃣ Verify Everything Works

- [ ] Open http://localhost:3000
- [ ] Create a new customer
- [ ] Create a new product
- [ ] Create an order
- [ ] Watch the order status update in real-time! 🎉

## 🔧 Configuration

### Environment Variables

#### Backend (.env)

```bash
# Database
ConnectionStrings__DefaultConnection=Host=localhost;Database=orders;Username=postgres;Password=yourpassword

# Azure Service Bus
ConnectionStrings__ServiceBus=Endpoint=sb://your-namespace.servicebus.windows.net/;SharedAccessKeyName=...

# Worker
WORKER_URLS=http://localhost:5001

# CORS
CORS_ORIGINS=http://localhost:3000
```

#### Frontend (.env.local)

```bash
# API URL
NEXT_PUBLIC_API_URL=http://localhost:5000

# SignalR Hub URL
NEXT_PUBLIC_SIGNALR_URL=http://localhost:5001/hubs/orders
```

## 📚 Project Structure

### Backend

```
backend/
├── src/
│   ├── Module/
│   │   └── Order/
│   │       ├── Application/      # Use cases
│   │       ├── Domain/           # Domain entities & business logic & events
│   │       └── Infrastructure/   # Data access & external services
│   └── Shared/
│       └── Infrastructure/
│           ├── Api/              # REST API service
│           └── Worker/           # Background worker + SignalR
└── docker-compose.dev.yml
```

### Frontend

```
frontend/
├── app/                          # Next.js app directory
├── src/
│   ├── component/                # React components
│   │   ├── atom/                # Base components
│   │   ├── molecule/            # Composite components
│   │   ├── organism/            # Complex components
│   │   └── template/            # Page templates
│   ├── lib/
│   │   ├── api/                 # API client & interfaces
│   │   ├── hook/                # Custom React hooks
│   │   ├── service/             # SignalR service
│   │   └── type/                # TypeScript types
│   └── util/                    # Utility functions
└── package.json
```

## 🎮 Usage

### Creating an Order

1. **Navigate to Orders tab** 📋
2. **Click "Create Order"** ➕
3. **Select a customer** 👤
4. **Add products and quantities** 🛒
5. **Submit** ✅

### Real-time Updates

Watch as your order progresses:

```
🟡 Pending      ─────▶  🟠 Processing  ─────▶  🟢 Done
    Created              After ~5s              After ~10s
```

All updates happen **automatically** without refreshing the page! ✨

## 🧪 Testing

### Backend Tests

```bash
cd backend
dotnet test
```

### Frontend Tests

```bash
cd frontend
npm test
```

### Health Checks

```bash
# API Health
curl http://localhost:5000/health

# Worker Health
curl http://localhost:5001/health
```

## 📊 API Endpoints

### Customers

- `GET /clients` - List customers (paginated)
- `POST /clients` - Create customer

### Products

- `GET /products` - List products (paginated)
- `POST /products` - Create product

### Orders

- `GET /orders` - List orders (paginated, filterable by status)
- `GET /orders/{id}` - Get order details
- `POST /orders` - Create order

### Health

- `GET /health` - Overall health status
- `GET /health/ready` - Readiness check
- `GET /health/live` - Liveness check

## 🔌 SignalR Events

### Hub Endpoint

```
ws://localhost:5001/hubs/orders
```

### Events

- **OrderStatusChanged** - Fired when order status updates
  ```json
  {
    "orderId": "uuid",
    "status": "Processing",
    "timestamp": "2025-01-08T...",
    "data": {
      "totalValue": 100.0,
      "currency": "USD",
      "updatedAt": "2025-01-08T..."
    }
  }
  ```

## ⚠️ Known Limitations & Missing Features

While the core functionality is working, the following features and improvements are missing or incomplete:

### Frontend Issues

#### 🔴 Critical
- **No Error Feedback** - API errors are not displayed to the user
  - Form submissions fail silently when validation errors occur
  - Network errors are not shown
  - SignalR connection failures are not communicated
- **No Loading States** - Missing loading indicators during async operations
  - Order creation shows no feedback while processing
  - Data fetching has no loading UI

#### 🟡 Important
- **Form Validation** - Client-side validation is minimal or missing
  - No real-time field validation
  - Error messages don't highlight specific fields
- **SignalR Error Handling** - Connection errors are logged but not shown to users
- **No Retry Logic** - Failed requests are not retried automatically

#### 🟢 Nice to Have
- **Toast Notifications** - Success/error messages should use toast UI
- **Optimistic Updates** - UI should update optimistically before server confirms
- **Empty States** - Better UI when lists are empty
- **Pagination Controls** - Current pagination is API-only, no UI controls
- **Confirmation Dialogs** - No confirmation before destructive actions

### Backend Gaps

#### 🔴 Critical
- **No Global Error Handler** - API lacks centralized exception handling middleware
  - Unhandled exceptions return generic 500 errors
  - No consistent error response format
  - Stack traces may leak to production
  - No error logging/monitoring integration

#### 🟡 Important
- **Authentication/Authorization** - No security layer implemented
- **Input Validation** - Some endpoints lack comprehensive validation
- **Error Responses** - Error messages could be more descriptive
- **Rate Limiting** - No protection against abuse

#### 🟢 Nice to Have
- **Audit Logging** - Order audit events are stored but not exposed via API
- **Request/Response Logging** - No middleware to log HTTP traffic

### Testing

- **No Frontend Tests** - No unit, integration, or E2E tests for React components
- **No Backend Tests** - Test infrastructure exists but no actual tests implemented
- **No Load Testing** - SignalR performance under load is untested

### DevOps

- **No CI/CD Pipeline** - No automated build/test/deploy workflow
- **No Monitoring** - No application performance monitoring (APM)
- **No Logging Aggregation** - Logs are local only
- **Docker Production Config** - Current docker-compose is dev-only

### Recommendations

To improve the application, prioritize in this order:

1. **Add Global Error Handler (API)** - Implement exception handling middleware
2. **Add Error Toast Notifications (Frontend)** - Implement toast library (e.g., react-hot-toast)
3. **Add Loading States (Frontend)** - Show spinners during async operations
4. **Improve Form Validation** - Add comprehensive client-side validation
5. **Implement Error Boundaries (Frontend)** - Catch React errors gracefully
6. **Add Authentication** - Implement JWT-based auth
7. **Write Tests** - Start with critical path E2E tests
8. **Add Monitoring** - Implement APM and error tracking

## 🐳 Running with Docker Compose

The easiest way to run the entire application stack is using Docker Compose. This will start all services (database, message queue, backend, and frontend) with a single command.

### Prerequisites

- [Docker](https://docs.docker.com/get-docker/) 20.10+
- [Docker Compose](https://docs.docker.com/compose/install/) 2.0+

### Step-by-Step Guide

#### 1️⃣ Clone the Repository

```bash
git clone https://github.com/felipessac/get-me-my-order.git
cd get-me-my-order
```

#### 2️⃣ Configure Environment Variables

The project includes a `.env` file with default configuration. You can use it as-is for development or modify it:

```bash
# Review and update if needed
cat .env

# Key variables:
# - Database credentials
# - Service Bus connection
# - CORS origins
# - Service URLs
```

**Important:** The `.env` file is already configured for Docker Compose. The default values will work out of the box.

#### 3️⃣ Build and Start All Services

```bash
# Build and start all services in detached mode
docker compose up -d --build
```

This command will:
- Build Docker images for API, Worker, and Frontend
- Start PostgreSQL database
- Start MS SQL Server (for Service Bus emulator)
- Start Azure Service Bus emulator
- Start API service on port 5000
- Start Worker service (SignalR) on port 5001
- Start Frontend on port 3000

#### 4️⃣ Monitor Service Startup

```bash
# View logs from all services
docker compose logs -f

# Or view logs from a specific service
docker compose logs -f api
docker compose logs -f worker
docker compose logs -f web
```

Wait until you see:
- `✅ API running` - API service is ready
- `✅ Worker running` - Worker service is ready
- `✅ Frontend compiled` - Frontend is ready

#### 5️⃣ Verify Services are Running

```bash
# Check all containers are up
docker compose ps

# Check health status
curl http://localhost:5000/health  # API health
curl http://localhost:5001/health  # Worker health
```

All services should show status as "Up (healthy)".

#### 6️⃣ Access the Application

Open your browser and navigate to:

- **Frontend**: http://localhost:3000
- **API Swagger**: http://localhost:5000/swagger
- **PgAdmin**: http://localhost:5050 (login: admin@admin.com / admin)

#### 7️⃣ Test the Application

1. Navigate to http://localhost:3000
2. Create a customer
3. Create products
4. Create an order
5. Watch real-time status updates! 🎉

### Managing the Application

#### Stop All Services

```bash
# Stop all services but keep data
docker compose stop

# Stop and remove containers (keeps volumes/data)
docker compose down

# Stop, remove containers AND volumes (fresh start)
docker compose down -v
```

#### Restart Services

```bash
# Restart all services
docker compose restart

# Restart a specific service
docker compose restart api
docker compose restart worker
docker compose restart web
```

#### View Service Logs

```bash
# All services
docker compose logs -f

# Specific service
docker compose logs -f api

# Last 100 lines
docker compose logs --tail 100 api
```

#### Rebuild After Code Changes

```bash
# Rebuild and restart all services
docker compose up -d --build

# Rebuild specific service
docker compose up -d --build api
```

### Useful Commands

```bash
# Execute commands inside a container
docker compose exec api bash
docker compose exec postgres psql -U postgres -d gmmo_db

# Check resource usage
docker compose stats

# Remove all containers, networks, and volumes
docker compose down -v --remove-orphans

# Pull latest images
docker compose pull
```

### Troubleshooting

#### Services Won't Start

```bash
# Check logs for errors
docker compose logs

# Restart with fresh state
docker compose down -v
docker compose up -d --build
```

#### Port Already in Use

If you see port binding errors, make sure ports 3000, 5000, 5001, and 5432 are not in use:

```bash
# Linux/Mac
lsof -i :3000
lsof -i :5000

# Windows
netstat -ano | findstr :3000
```

#### Database Connection Issues

```bash
# Check if PostgreSQL is healthy
docker compose ps postgres

# Access database directly
docker compose exec postgres psql -U postgres -d gmmo_db

# View database logs
docker compose logs postgres
```

#### Reset Everything

```bash
# Complete reset (removes all data)
docker compose down -v
docker system prune -a
docker compose up -d --build
```

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Authors

- **Your Name** - _Initial work_ - [@felipessac](https://github.com/felipessac)

## 🙏 Acknowledgments

- Built using modern technologies
- Inspired by clean architecture principles
- Special thanks to the open-source community

## 📞 Support

- 📧 Email: felipe_adoubs@outlook.com
- 🐛 Issues: [GitHub Issues](https://github.com/felipessac/get-me-my-order/issues)
