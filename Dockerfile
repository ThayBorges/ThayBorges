# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY WasteManagement.sln ./
COPY src/WasteManagement.Domain/WasteManagement.Domain.csproj src/WasteManagement.Domain/
COPY src/WasteManagement.Application/WasteManagement.Application.csproj src/WasteManagement.Application/
COPY src/WasteManagement.Infrastructure/WasteManagement.Infrastructure.csproj src/WasteManagement.Infrastructure/
COPY src/WasteManagement.Api/WasteManagement.Api.csproj src/WasteManagement.Api/
COPY tests/WasteManagement.Tests/WasteManagement.Tests.csproj tests/WasteManagement.Tests/

RUN dotnet restore WasteManagement.sln

COPY . .
RUN dotnet publish src/WasteManagement.Api/WasteManagement.Api.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "WasteManagement.Api.dll"]
