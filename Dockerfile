
# Build de l'application ASP.NET Core en .NET 9
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY FirstProject.csproj ./
RUN dotnet restore FirstProject.csproj

COPY . .
RUN dotnet publish FirstProject.csproj -c Release -o /app/publish /p:UseAppHost=false

# Image d'exécution
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "FirstProject.dll"]
