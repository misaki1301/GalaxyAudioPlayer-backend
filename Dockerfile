﻿FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["GalaxyAudioPlayer.csproj", "./"]
RUN dotnet restore "GalaxyAudioPlayer.csproj"
COPY . .
WORKDIR "/src/GalaxyAudioPlayer"
RUN dotnet build "GalaxyAudioPlayer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GalaxyAudioPlayer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GalaxyAudioPlayer.dll"]