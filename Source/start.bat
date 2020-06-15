docker build -t aot-api:latest Aot.Hrms.Api
docker build -t aot-api:latest .
docker run -p 127.0.0.1:61653:80/tcp aot-web