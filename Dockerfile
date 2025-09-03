# Stage 1: Build Environment
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copy everything at once
COPY . .

# Restore dependencies for the entire solution
RUN dotnet restore "MuebleriaBack.sln"

# Publish the application from the root, specifying the project file
RUN dotnet publish "WebApi/WebApi.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Runtime Environment
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# The default port for ASP.NET Core is 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "WebApi.dll"]
