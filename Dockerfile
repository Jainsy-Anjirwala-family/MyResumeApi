# STAGE 1: Build the app
# Use the official .NET 8 SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file first to install dependencies (Cached layer)
COPY ["MyResumeApi.csproj", "./"]
RUN dotnet restore "MyResumeApi.csproj"

# Copy the rest of the code and build the release version
COPY . .
RUN dotnet publish "MyResumeApi.csproj" -c Release -o /app/publish

# STAGE 2: Run the app
# Use the lighter ASP.NET runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# CONFIGURE PORT FOR RENDER
# Render assigns a random port, but we force the app to listen on 8080
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "MyResumeApi.dll"]