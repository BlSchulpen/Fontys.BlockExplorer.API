#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Fontys.BlockExplorer.API/Fontys.BlockExplorer.API.csproj", "Fontys.BlockExplorer.API/"]
COPY ["Fontys.BlockExplorer.Data/Fontys.BlockExplorer.Data.csproj", "Fontys.BlockExplorer.Data/"]
COPY ["Fontys.BlockExplorer.Domain/Fontys.BlockExplorer.Domain.csproj", "Fontys.BlockExplorer.Domain/"]
COPY ["Fontys.BlockExplorer.Application/Fontys.BlockExplorer.Application.csproj", "Fontys.BlockExplorer.Application/"]
COPY ["Fontys.BlockExplorer.NodeWarehouse/Fontys.BlockExplorer.NodeWarehouse.csproj", "Fontys.BlockExplorer.NodeWarehouse/"]
RUN dotnet restore "Fontys.BlockExplorer.API/Fontys.BlockExplorer.API.csproj"
COPY . .
WORKDIR "/src/Fontys.BlockExplorer.API"
RUN dotnet build "Fontys.BlockExplorer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fontys.BlockExplorer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fontys.BlockExplorer.API.dll"]