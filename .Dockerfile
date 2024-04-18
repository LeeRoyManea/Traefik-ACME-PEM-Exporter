# Use the official .NET Core SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . ./

RUN dotnet restore
 
RUN dotnet publish -c Release -o out
  
RUN dotnet restore "TraefikExporter/TraefikExporter.csproj"
RUN dotnet build "TraefikExporter/TraefikExporter.csproj" -c Release
RUN dotnet publish "TraefikExporter/TraefikExporter.csproj" -c Release -o out

 # Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "TraefikExporter.dll"]

ENV TZ=Europe/Zurich
ENV DEBIAN_FRONTEND=noninteractive
ENV AcmePath="/data/acme.json"
ENV ExportPath="/data/export"
ENV TimeInterval=15