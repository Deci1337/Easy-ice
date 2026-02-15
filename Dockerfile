# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["src/Backend/EasyIce.WebApi/EasyIce.WebApi.csproj", "Backend/EasyIce.WebApi/"]
RUN dotnet restore "Backend/EasyIce.WebApi/EasyIce.WebApi.csproj"

# Copy everything else and build
COPY src/Backend/EasyIce.WebApi/. Backend/EasyIce.WebApi/
WORKDIR "/src/Backend/EasyIce.WebApi"
RUN dotnet build "EasyIce.WebApi.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "EasyIce.WebApi.csproj" -c Release -o /app/publish

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EasyIce.WebApi.dll"]
