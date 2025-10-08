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

## 🐳 Docker

### Run with Docker Compose

```bash
cd backend
docker-compose -f docker-compose.dev.yml up
```

This will start:

- PostgreSQL database
- Azure Service Bus emulator (Azurite)
- API service
- Worker service

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
