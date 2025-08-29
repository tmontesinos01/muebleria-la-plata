# Use the .NET 8 SDK as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy all the files from the local source to the image
COPY . .

# Restore dependencies for the entire solution
RUN dotnet restore "MuebleriaBack.sln"

# Publish the main web application project
RUN dotnet publish "WebApi/WebApi.csproj" -c Release -o /app/publish --no-restore

# Use the .NET 8 ASP.NET runtime as the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Set the entrypoint for the container
ENTRYPOINT ["dotnet", "WebApi.dll"]
