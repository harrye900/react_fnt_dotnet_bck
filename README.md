# Microservices E-Commerce Application

## Architecture Overview

This application demonstrates a microservices architecture with:

- **Frontend**: React.js (Port 3000)
- **API Gateway**: Routes requests to services (Port 5000)
- **Auth Service**: Handles login/authentication (Port 5001)
- **Product Service**: Manages product catalog (Port 5002)
- **Cart Service**: Shopping cart functionality (Port 5003)
- **Payment Service**: Payment processing (Port 5004)
- **Order Service**: Order management (Port 5005)

## How Services Communicate

1. **Frontend → API Gateway**: React app sends all requests to port 5000
2. **API Gateway → Services**: Routes requests to appropriate microservices
3. **Service-to-Service**: Order Service calls Payment Service directly
4. **Data Flow**: Login → Browse Products → Add to Cart → Checkout → Payment → Order

## Running the Application

### Option 1: Docker (Recommended)
```bash
# Prerequisites: Docker Desktop
# Build images
docker-build.bat

# Run containers
docker-run-containers.bat

# Stop containers
docker-stop.bat
```

### Option 2: Local Development
#### Prerequisites
- .NET 8.0 SDK
- Node.js and npm
- MongoDB Community Server (localhost:27017)

#### Setup MongoDB
```bash
# Install MongoDB and ensure it's running on localhost:27017
setup-mongodb.bat
```

#### Start Backend Services
```bash
# Run this in the root directory
start-services.bat
```

### Start Frontend
```bash
# Run this in a separate terminal
start-frontend.bat
```

### Access the Application
- Frontend: http://localhost:3000
- API Gateway: http://localhost:5000

## Test the Flow

1. Click "Login" (uses user@test.com/password)
2. Browse products from Product Service
3. Add items to cart (Cart Service)
4. Click "Cart" to view items
5. Click "Place Order" (triggers Payment → Order services)

## Service Communication Examples

- **API Gateway** routes `/api/gateway/products` → Product Service
- **Order Service** calls Payment Service during checkout
- **Cart Service** stores user cart data in memory
- **Auth Service** validates login credentials

Each service runs independently and can be scaled separately.