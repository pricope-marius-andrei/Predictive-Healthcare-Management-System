# Use the latest .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
EXPOSE 80
EXPOSE 8081

# Copy the main project and all referenced projects
COPY ["Predictive-Healthcare-Management-System/Predictive-Healthcare-Management-System.csproj", "./Predictive-Healthcare-Management-System/"]
COPY ["Application/Application.csproj", "./Application/"]
COPY ["Domain/Domain.csproj", "./Domain/"]
COPY ["Infrastracture/Infrastructure.csproj", "./Infrastracture/"]
COPY ["Predictive-Healthcare-Management-System.Application.UnitTests/Predictive-Healthcare-Management-System.Application.UnitTests.csproj", "./Predictive-Healthcare-Management-System.Application.UnitTests/"]
COPY ["Predictive-Healthcare-Management-System.IntegrationTests/Predictive-Healthcare-Management-System.IntegrationTests.csproj", "./Predictive-Healthcare-Management-System.IntegrationTests/"]

# Restore dependencies
RUN dotnet restore "Predictive-Healthcare-Management-System/Predictive-Healthcare-Management-System.csproj"

# Copy the remaining source code
COPY . .

# Build and publish the application
WORKDIR /app/Predictive-Healthcare-Management-System
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

# Use the runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Predictive-Healthcare-Management-System.dll"]

