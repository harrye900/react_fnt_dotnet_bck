@echo off
echo Starting all microservices...

start "API Gateway" cmd /k "cd backend\ApiGateway && dotnet run"
timeout /t 2 /nobreak >nul

start "Auth Service" cmd /k "cd backend\AuthService && dotnet run"
timeout /t 2 /nobreak >nul

start "Product Service" cmd /k "cd backend\ProductService && dotnet run"
timeout /t 2 /nobreak >nul

start "Cart Service" cmd /k "cd backend\CartService && dotnet run"
timeout /t 2 /nobreak >nul

start "Payment Service" cmd /k "cd backend\PaymentService && dotnet run"
timeout /t 2 /nobreak >nul

start "Order Service" cmd /k "cd backend\OrderService && dotnet run"
timeout /t 2 /nobreak >nul

echo All services started!
echo API Gateway: http://localhost:5000
echo Auth Service: http://localhost:5001
echo Product Service: http://localhost:5002
echo Cart Service: http://localhost:5003
echo Payment Service: http://localhost:5004
echo Order Service: http://localhost:5005

pause