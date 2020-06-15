#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Aot.Hrms.Api/Aot.Hrms.Api.csproj", "Aot.Hrms.Api/"]
COPY ["Aot.Hrms.Repositories/Aot.Hrms.Repositories.csproj", "Aot.Hrms.Repositories/"]
COPY ["Aot.Hrms.Entities/Aot.Hrms.Entities.csproj", "Aot.Hrms.Entities/"]
COPY ["Aot.Hrms.Contracts/Aot.Hrms.Contracts.csproj", "Aot.Hrms.Contracts/"]
COPY ["Aot.Hrms.Dtos/Aot.Hrms.Dtos.csproj", "Aot.Hrms.Dtos/"]
COPY ["Aot.Hrms.Services/Aot.Hrms.Services.csproj", "Aot.Hrms.Services/"]
RUN dotnet restore "Aot.Hrms.Api/Aot.Hrms.Api.csproj"
COPY . .
WORKDIR "/src/Aot.Hrms.Api"
RUN dotnet build "Aot.Hrms.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Aot.Hrms.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Aot.Hrms.Api.dll"]
