# Multi-stage build for PrimeNest API
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy sln and project files
COPY ["PrimeNest/PrimeNest.sln", "."]
COPY ["PrimeNest/Core/Core.csproj", "Core/"]
COPY ["PrimeNest/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["PrimeNest/ProjectApi/ProjectApi.csproj", "ProjectApi/"]
COPY ["PrimeNest/ProjectApi.Test/ProjectApi.Test.csproj", "ProjectApi.Test/"]

# Restore dependencies
RUN dotnet restore "PrimeNest.sln"

# Copy all source files
COPY ["PrimeNest/", "."]

# Build the application
RUN dotnet build "PrimeNest.sln" -c Release -o /app/build

# Stage 2: Test
FROM build AS test
WORKDIR /src

# Run unit tests
RUN dotnet test "ProjectApi.Test/ProjectApi.Test.csproj" --no-build --verbosity normal --logger "console;verbosity=detailed"

# Stage 3: Publish
FROM build AS publish
WORKDIR /src

# Publish the application
RUN dotnet publish "ProjectApi/ProjectApi.csproj" -c Release -o /app/publish

# Stage 4: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published files from publish stage
COPY --from=publish /app/publish .

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
