@echo off
echo Building Docker images...

docker build -t api-gateway ./backend/ApiGateway
docker build -t auth-service ./backend/AuthService
docker build -t product-service ./backend/ProductService
docker build -t cart-service ./backend/CartService
docker build -t payment-service ./backend/PaymentService
docker build -t order-service ./backend/OrderService

echo.
echo Images built successfully!
echo - api-gateway
echo - auth-service
echo - product-service
echo - cart-service
echo - payment-service
echo - order-service
echo.
pause