# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project files to the container
COPY ["CyberStoreSVC/CyberStoreSVC.csproj", "CyberStoreSVC/"]
RUN dotnet restore "CyberStoreSVC/CyberStoreSVC.csproj"

# Copy the rest of the code and build it
COPY . .
WORKDIR "/src/CyberStoreSVC"
RUN dotnet build "CyberStoreSVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CyberStoreSVC.csproj" -c Release -o /app/publish

# Final stage to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CyberStoreSVC.dll"]