# EntityApp/mocks/MockExportService/Dockerfile
FROM mcr.microsoft.com/dotnet/8.0-aspnet AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/8.0-sdk AS build
WORKDIR /src
COPY ["mocks/MockExportService/MockExportService.csproj", "mocks/MockExportService/"]
RUN dotnet restore "mocks/MockExportService/MockExportService.csproj"
COPY mocks/MockExportService/ mocks/MockExportService/
WORKDIR /src/mocks/MockExportService
RUN dotnet build "MockExportService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MockExportService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MockExportService.dll"]