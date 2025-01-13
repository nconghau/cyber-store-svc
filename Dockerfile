# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files and restore dependencies
COPY DotnetApiPostgres.Api.csproj ./
RUN dotnet restore

# Copy the rest of the application files and build
COPY . ./
RUN dotnet publish -c Release -o out

# Use the ASP.NET Core runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Set the environment variable to Development (this can be adjusted if necessary)
ENV ASPNETCORE_ENVIRONMENT Development

# Expose both HTTP and HTTPS ports as per the launchSettings.json
EXPOSE 5106
EXPOSE 7294

# Start the application
ENTRYPOINT ["dotnet", "DotnetApiPostgres.Api.dll"]
