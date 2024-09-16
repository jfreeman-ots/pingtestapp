# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PingApp.csproj", "./"]
RUN dotnet restore "PingApp.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "PingApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PingApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PingApp.dll"]
