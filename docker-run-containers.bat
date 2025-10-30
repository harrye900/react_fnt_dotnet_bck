@echo off
echo Starting containers...

REM Create network
docker network create microservices-net 2>nul

REM Start MongoDB
docker run -d --name mongodb --network microservices-net -p 27017:27017 mongo:7.0

REM Wait for MongoDB
timeout /t 5 /nobreak >nul

REM Start Auth Service
docker run -d --name auth-service --network microservices-net -p 5001:5001 auth-service

REM Start Product Service
docker run -d --name product-service --network microservices-net -p 5002:5002 -e MONGO_CONNECTION=mongodb://mongodb:27017 product-service

REM Start Cart Service
docker run -d --name cart-service --network microservices-net -p 5003:5003 cart-service

REM Start Payment Service
docker run -d --name payment-service --network microservices-net -p 5004:5004 payment-service

REM Start Order Service  
docker run -d --name order-service --network microservices-net -p 5005:5005 -e MONGO_CONNECTION=mongodb://mongodb:27017 order-service

REM Start API Gateway
docker run -d --name api-gateway --network microservices-net -p 5000:5000 -e AUTH_SERVICE_URL=http://auth-service:5001 -e PRODUCT_SERVICE_URL=http://product-service:5002 -e CART_SERVICE_URL=http://cart-service:5003 -e PAYMENT_SERVICE_URL=http://payment-service:5004 -e ORDER_SERVICE_URL=http://order-service:5005 api-gateway

echo.
echo Containers started!
echo API Gateway: http://localhost:5000
echo Product Service: http://localhost:5002
echo Order Service: http://localhost:5005
echo MongoDB: localhost:27017
echo.
pause