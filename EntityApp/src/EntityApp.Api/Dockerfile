# EntityApp/src/EntityApp.Api/Dockerfile
FROM mcr.microsoft.com/dotnet/8.0-aspnet AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/8.0-sdk AS build
WORKDIR /src
COPY ["src/EntityApp.Api/EntityApp.Api.csproj", "src/EntityApp.Api/"]
RUN dotnet restore "src/EntityApp.Api/EntityApp.Api.csproj"
COPY src/EntityApp.Api/ src/EntityApp.Api/
WORKDIR /src/src/EntityApp.Api
RUN dotnet build "EntityApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EntityApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EntityApp.Api.dll"]