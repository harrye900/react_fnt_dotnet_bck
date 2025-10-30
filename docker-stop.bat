@echo off
echo Stopping and removing containers...

docker stop api-gateway auth-service product-service cart-service payment-service order-service mongodb 2>nul
docker rm api-gateway auth-service product-service cart-service payment-service order-service mongodb 2>nul
docker network rm microservices-net 2>nul

echo.
echo All containers stopped and removed!
echo.
pause