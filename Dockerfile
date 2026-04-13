# Multi-stage build for PrimeNest API
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all source files first
COPY ["PrimeNest/", "."]

# Restore dependencies
RUN dotnet restore "PrimeNest.sln"

# Publish the application
RUN dotnet publish "ProjectApi/ProjectApi.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish .

# Create directories for file uploads
RUN mkdir -p wwwroot/ProfilePhoto

# Expose ports
EXPOSE 80
EXPOSE 443

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD dotnet exec /app/ProjectApi.dll || exit 1

# Run the application
ENTRYPOINT ["dotnet", "ProjectApi.dll"]
