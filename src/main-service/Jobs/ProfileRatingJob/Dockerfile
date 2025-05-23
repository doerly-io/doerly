FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Jobs/ProfileRatingJob/ProfileRatingJob.csproj", "Jobs/ProfileRatingJob/"]
RUN dotnet restore "Jobs/ProfileRatingJob/ProfileRatingJob.csproj"
COPY . .
WORKDIR "/src/Jobs/ProfileRatinJob"
RUN dotnet build "ProfileRatingJob.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProfileRatingJob.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProfileRatingJob.dll"]
