# TestLocal:: the official .NET SDK as a base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the .csproj file to the container's working directory
COPY CyberStoreSVC.csproj ./CyberStoreSVC.csproj

# Restore the NuGet packages
RUN dotnet restore "./CyberStoreSVC.csproj"

# Copy the rest of the project files to the container
COPY . .

# Build the project
RUN dotnet build "CyberStoreSVC.csproj" -c Release -o /app/build

# Publish the project to the /app/publish folder
RUN dotnet publish "CyberStoreSVC.csproj" -c Release -o /app/publish

# Set environment variables for the container
ENV ASPNETCORE_ENVIRONMENT=Development \
    PostgresConnection="Host=14.225.204.163;Port=5332;Database=cyber_store;Username=cyber_store;Password=cyber_store" \
    KafkaBroker="14.225.204.163:9092" \
    KafkaTopic="k_order_2" \
    KafkaNumPartitions="4" \
    KafkaNumConsumers="4" \
    KafkaGroupId="k_order_2_groupId" \
    AuthFilterToken="token-nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCokUihg-staging" \
    ASPNETCORE_URLS="http://+:7295"
    # ASPNETCORE_URLS="http://+:80;https://+:443"  

# TestLocal::Expose port 7295
EXPOSE 7295

# Expose ports 80 and 443 for HTTP and HTTPS access
# EXPOSE 80
# EXPOSE 443

# Set the entry point for the container
ENTRYPOINT ["dotnet", "/app/publish/CyberStoreSVC.dll"]

# TestLocal::Srcipt
# docker build -t cyber-store-svc .
# docker run -d -p 7295:7295 --name cyber-store-svc cyber-store-svc
# docker run -d -p 80:80 -p 443:443 --name cyber-store-svc cyber-store-svc

# Build VPS
# docker login --username=haucoder
# docker buildx build --platform linux/amd64 -t haucoder/cyber-store-svc:1.0.0 .
# docker push haucoder/cyber-store-svc:1.0.0