# Use the .NET SDK image based on Ubuntu
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

# Copy the project file and restore dependencies
COPY Case_Study/*.csproj ./Case_Study/
RUN dotnet restore ./Case_Study/*.csproj

# Copy the remaining source code and publish the application
COPY . ./
RUN dotnet publish Case_Study -c Release -o out

# Build the docker image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Set the port of the application to 80
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "GymApp.dll"]

