@echo off
echo Building and starting containerized microservices...
echo.

docker-compose down
docker-compose build
docker-compose up -d

echo.
echo Services starting...
echo API Gateway: http://localhost:5000
echo Product Service: http://localhost:5002
echo Order Service: http://localhost:5005
echo MongoDB: localhost:27017
echo.
echo Use 'docker-compose logs -f' to view logs
echo Use 'docker-compose down' to stop services
echo.
pause