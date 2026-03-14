FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["NotificationsAPI.Api/NotificationsAPI.Api.csproj", "NotificationsAPI.Api/"]
COPY ["NotificationsAPI.Application/NotificationsAPI.Application.csproj", "NotificationsAPI.Application/"]
RUN dotnet restore "NotificationsAPI.Api/NotificationsAPI.Api.csproj"
COPY . .
WORKDIR "/src/NotificationsAPI.Api"
RUN dotnet build "NotificationsAPI.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotificationsAPI.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationsAPI.Api.dll"]
