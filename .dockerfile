# Use the official .NET Core SDK image
ENV TZ=Europe/Zurich
ENV DEBIAN_FRONTEND=noninteractive
ENV AcmePath="/data/acme.json"
ENV ExportPath="/data/export"
ENV TimeInterval=15

FROM mcr.microsoft.com/dotnet/core/sdk:8.0 AS build
WORKDIR /app
  
# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore
  
 # Copy everything else and build the application
COPY . ./
RUN dotnet publish -c Release -o out
  
 # Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "TraefikExporter.dll"]