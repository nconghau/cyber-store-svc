# 🏗️ **Stage 1: Build the application**
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy only the project file (to leverage Docker caching)
COPY CyberStoreSVC.csproj ./

# Restore dependencies
RUN dotnet restore "./CyberStoreSVC.csproj"

# Copy the rest of the project files
COPY . .

# Build the application
RUN dotnet build "CyberStoreSVC.csproj" -c Release -o /app/build

# Publish the application (creates a self-contained app)
RUN dotnet publish "CyberStoreSVC.csproj" -c Release -o /app/publish

# 🏗️ **Stage 2: Run the application with a lightweight runtime**
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Set the working directory
WORKDIR /app

# Copy published files from the build stage
COPY --from=build /app/publish .

# Copy the Private directory from the build stage
COPY Private/cyber_store_gcp_logs.json /app/Private/cyber_store_gcp_logs.json

# 🔹 Load environment variables from `.env` (via `docker-compose`)
ENV ASPNETCORE_URLS="http://+:7295"

# Expose port 7295
EXPOSE 7295

# Add HEALTHCHECK for zero-downtime
#HEALTHCHECK --interval=10s --timeout=3s --start-period=30s --retries=3 \
 # CMD curl -f http://localhost:7295/api/test/ping || exit 1

# Set the entry point
ENTRYPOINT ["dotnet", "CyberStoreSVC.dll"]
