# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file and restore the dependencies
COPY ["InWestyGator.WebDemo/InWestyGator.WebDemo.csproj", "InWestyGator.WebDemo/"]
RUN dotnet restore "InWestyGator.WebDemo/InWestyGator.WebDemo.csproj"

# Copy the remaining files and build the application
COPY . .
WORKDIR "/src/InWestyGator.WebDemo"
RUN dotnet build "InWestyGator.WebDemo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InWestyGator.WebDemo.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /
COPY --from=publish /app/publish ./app
ENTRYPOINT ["dotnet", "./app/InWestyGator.WebDemo.dll"]
