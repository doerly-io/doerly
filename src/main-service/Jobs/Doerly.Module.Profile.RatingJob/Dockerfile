FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine AS base
USER $APP_UID
WORKDIR /app

# Add ICU globalization support
USER root
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Jobs/Doerly.Module.Profile.RatingJob/Doerly.Module.Profile.RatingJob.csproj", "Jobs/Doerly.Module.Profile.RatingJob/"]
RUN dotnet restore "Jobs/Doerly.Module.Profile.RatingJob/Doerly.Module.Profile.RatingJob.csproj"
COPY . .
WORKDIR "/src/Jobs/Doerly.Module.Profile.RatingJob"
RUN dotnet build "./Doerly.Module.Profile.RatingJob.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Doerly.Module.Profile.RatingJob.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Doerly.Module.Profile.RatingJob.dll"]
