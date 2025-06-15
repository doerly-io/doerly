# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Add ICU globalization support
USER root
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Doerly.Host/Doerly.Host.csproj", "Doerly.Host/"]
COPY ["Doerly.Domain/Doerly.Domain.csproj", "Doerly.Domain/"]
COPY ["Doerly.DataAccess/Doerly.DataAccess.csproj", "Doerly.DataAccess/"]
COPY ["Doerly.DataTransferObjects/Doerly.DataTransferObjects.csproj", "Doerly.DataTransferObjects/"]
COPY ["Doerly.Extensions/Doerly.Extensions.csproj", "Doerly.Extensions/"]
COPY ["Doerly.FileRepository/Doerly.FileRepository.csproj", "Doerly.FileRepository/"]
COPY ["Doerly.Common.Settings/Doerly.Common.Settings.csproj", "Doerly.Common.Settings/"]
COPY ["Doerly.Localization/Doerly.Localization.csproj", "Doerly.Localization/"]
COPY ["Doerly.Common.Helpers/Doerly.Common.Helpers.csproj", "Doerly.Common.Helpers/"]
COPY ["Doerly.Messaging/Doerly.Messaging.csproj", "Doerly.Messaging/"]
COPY ["Doerly.Module.Communication/Doerly.Module.Communication.Api/Doerly.Module.Communication.Api.csproj", "Doerly.Module.Communication/Doerly.Module.Communication.Api/"]
COPY ["Doerly.Infrastructure.Api/Doerly.Infrastructure.Api.csproj", "Doerly.Infrastructure.Api/"]
COPY ["Doerly.Module.Communication/Doerly.Module.Communication.DataTransferObjects/Doerly.Module.Communication.DataTransferObjects.csproj", "Doerly.Module.Communication/Doerly.Module.Communication.DataTransferObjects/"]
COPY ["Doerly.Module.Profile/Doerly.Module.Profile.DataTransferObjects/Doerly.Module.Profile.DataTransferObjects.csproj", "Doerly.Module.Profile/Doerly.Module.Profile.DataTransferObjects/"]
COPY ["Doerly.Module.Profile/Doerly.Module.Profile.Enums/Doerly.Module.Profile.Enums.csproj", "Doerly.Module.Profile/Doerly.Module.Profile.Enums/"]
COPY ["Doerly.Module.Communication/Doerly.Module.Communication.Enums/Doerly.Module.Communication.Enums.csproj", "Doerly.Module.Communication/Doerly.Module.Communication.Enums/"]
COPY ["Doerly.Module.Communication/Doerly.Module.Communication.DataAccess/Doerly.Module.Communication.DataAccess.csproj", "Doerly.Module.Communication/Doerly.Module.Communication.DataAccess/"]
COPY ["Doerly.Module.Communication/Doerly.Module.Communication.Domain/Doerly.Module.Communication.Domain.csproj", "Doerly.Module.Communication/Doerly.Module.Communication.Domain/"]
COPY ["Doerly.Module.Notification/Doerly.Module.Notification.Enums/Doerly.Module.Notification.Enums.csproj", "Doerly.Module.Notification/Doerly.Module.Notification.Enums/"]
COPY ["Doerly.Proxy.Profile/Doerly.Proxy.Profile.csproj", "Doerly.Proxy.Profile/"]
COPY ["Doerly.Module.Order/Doerly.Module.Order.DataTransferObjects/Doerly.Module.Order.DataTransferObjects.csproj", "Doerly.Module.Order/Doerly.Module.Order.DataTransferObjects/"]
COPY ["Doerly.Module.Order/Doerly.Module.Order.Enums/Doerly.Module.Order.Enums.csproj", "Doerly.Module.Order/Doerly.Module.Order.Enums/"]
COPY ["Doerly.Proxy.BaseProxy/Doerly.Proxy.BaseProxy.csproj", "Doerly.Proxy.BaseProxy/"]
COPY ["Doerly.Module.Authorization/Doerly.Module.Authorization.Api/Doerly.Module.Authorization.Api.csproj", "Doerly.Module.Authorization/Doerly.Module.Authorization.Api/"]
COPY ["Doerly.Module.Authorization/Doerly.Module.Authorization.Domain/Doerly.Module.Authorization.Domain.csproj", "Doerly.Module.Authorization/Doerly.Module.Authorization.Domain/"]
COPY ["Doerly.Notification.EmailSender/Doerly.Notification.EmailSender.csproj", "Doerly.Notification.EmailSender/"]
COPY ["Doerly.Proxy.Authorization/Doerly.Proxy.Authorization.csproj", "Doerly.Proxy.Authorization/"]
COPY ["Doerly.Module.Authorization/Doerly.Module.Authorization.DataTransferObjects/Doerly.Module.Authorization.DataTransferObjects.csproj", "Doerly.Module.Authorization/Doerly.Module.Authorization.DataTransferObjects/"]
COPY ["Doerly.Module.Authorization/Doerly.Module.Authorization.DataAccess/Doerly.Module.Authorization.DataAccess.csproj", "Doerly.Module.Authorization/Doerly.Module.Authorization.DataAccess/"]
COPY ["Doerly.Module.Authorization/Doerly.Module.Authorization.Enums/Doerly.Module.Authorization.Enums.csproj", "Doerly.Module.Authorization/Doerly.Module.Authorization.Enums/"]
COPY ["Doerly.Module.Catalog/Doerly.Module.Catalog.Api/Doerly.Module.Catalog.Api.csproj", "Doerly.Module.Catalog/Doerly.Module.Catalog.Api/"]
COPY ["Doerly.Proxy.Catalog/Doerly.Proxy.Catalog.csproj", "Doerly.Proxy.Catalog/"]
COPY ["Doerly.Module.Catalog/Doerly.Module.Catalog.DataAccess/Doerly.Module.Catalog.DataAccess.csproj", "Doerly.Module.Catalog/Doerly.Module.Catalog.DataAccess/"]
COPY ["Doerly.Module.Catalog/Doerly.Module.Catalog.Enums/Doerly.Module.Catalog.Enums.csproj", "Doerly.Module.Catalog/Doerly.Module.Catalog.Enums/"]
COPY ["Doerly.Module.Catalog/Doerly.Module.Catalog.Domain/Doerly.Module.Catalog.Domain.csproj", "Doerly.Module.Catalog/Doerly.Module.Catalog.Domain/"]
COPY ["Doerly.Proxy.Orders/Doerly.Proxy.Orders.csproj", "Doerly.Proxy.Orders/"]
COPY ["Doerly.Module.Catalog/Doerly.Module.Catalog.Contracts/Doerly.Module.Catalog.Contracts.csproj", "Doerly.Module.Catalog/Doerly.Module.Catalog.Contracts/"]
COPY ["Doerly.Module.Common/Doerly.Module.Common.Api/Doerly.Module.Common.Api.csproj", "Doerly.Module.Common/Doerly.Module.Common.Api/"]
COPY ["Doerly.Module.Common/Doerly.Module.Common.DataAccess/Doerly.Module.Common.DataAccess.csproj", "Doerly.Module.Common/Doerly.Module.Common.DataAccess/"]
COPY ["Doerly.Module.Notification/Doerly.Module.Notification.Api/Doerly.Module.Notification.Api.csproj", "Doerly.Module.Notification/Doerly.Module.Notification.Api/"]
COPY ["Doerly.Module.Notification/Doerly.Module.Notification.Domain/Doerly.Module.Notification.Domain.csproj", "Doerly.Module.Notification/Doerly.Module.Notification.Domain/"]
COPY ["Doerly.Module.Notification/Doerly.Module.Notification.DataAccess/Doerly.Module.Notification.DataAccess.csproj", "Doerly.Module.Notification/Doerly.Module.Notification.DataAccess/"]
COPY ["Doerly.Module.Notification/Doerly.Module.Notification.DataTransferObjects/Doerly.Module.Notification.DataTransferObjects.csproj", "Doerly.Module.Notification/Doerly.Module.Notification.DataTransferObjects/"]
COPY ["Doerly.Module.Notification/Doerly.Module.Notification.Messaging/Doerly.Module.Notification.Messaging.csproj", "Doerly.Module.Notification/Doerly.Module.Notification.Messaging/"]
COPY ["Doerly.Module.Payments/Doerly.Module.Payments.DataTransferObjects/Doerly.Module.Payments.DataTransferObjects.csproj", "Doerly.Module.Payments/Doerly.Module.Payments.DataTransferObjects/"]
COPY ["Doerly.Module.Payments/Doerly.Module.Payments.Enums/Doerly.Module.Payments.Enums.csproj", "Doerly.Module.Payments/Doerly.Module.Payments.Enums/"]
COPY ["Doerly.Module.Profile/Doerly.Module.Profile.Api/Doerly.Module.Profile.Api.csproj", "Doerly.Module.Profile/Doerly.Module.Profile.Api/"]
COPY ["Doerly.Module.Profile/Doerly.Module.Profile.DataAccess/Doerly.Module.Profile.DataAccess.csproj", "Doerly.Module.Profile/Doerly.Module.Profile.DataAccess/"]
COPY ["Doerly.Module.Profile/Doerly.Module.Profile.Domain/Doerly.Module.Profile.Domain.csproj", "Doerly.Module.Profile/Doerly.Module.Profile.Domain/"]
COPY ["Doerly.Helpers/Doerly.Helpers.csproj", "Doerly.Helpers/"]
COPY ["Doerly.Proxy.Payment/Doerly.Proxy.Payment.csproj", "Doerly.Proxy.Payment/"]
COPY ["Doerly.Module.Order/Doerly.Module.Order.Api/Doerly.Module.Order.Api.csproj", "Doerly.Module.Order/Doerly.Module.Order.Api/"]
COPY ["Doerly.Module.Order/Doerly.Module.Order.DataAccess/Doerly.Module.Order.DataAccess.csproj", "Doerly.Module.Order/Doerly.Module.Order.DataAccess/"]
COPY ["Doerly.Module.Order/Doerly.Module.Order.Domain/Doerly.Module.Order.Domain.csproj", "Doerly.Module.Order/Doerly.Module.Order.Domain/"]
COPY ["Doerly.Module.Payments/Doerly.Module.Payments.Api/Doerly.Module.Payments.Api.csproj", "Doerly.Module.Payments/Doerly.Module.Payments.Api/"]
COPY ["Doerly.Module.Payments/Doerly.Module.Payments.BaseClient/Doerly.Module.Payments.BaseClient.csproj", "Doerly.Module.Payments/Doerly.Module.Payments.BaseClient/"]
COPY ["Doerly.Module.Payments/Doerly.Module.Payments.DataAccess/Doerly.Module.Payments.DataAccess.csproj", "Doerly.Module.Payments/Doerly.Module.Payments.DataAccess/"]
COPY ["Doerly.Module.Payments/Doerly.Module.Payments.Domain/Doerly.Module.Payments.Domain.csproj", "Doerly.Module.Payments/Doerly.Module.Payments.Domain/"]
COPY ["Doerly.Module.Payments/Doerly.Module.Payments.Client.LiqPay/Doerly.Module.Payments.Client.LiqPay.csproj", "Doerly.Module.Payments/Doerly.Module.Payments.Client.LiqPay/"]
COPY ["Doerly.Module.Statistics.Api/Doerly.Module.Statistics.Api.csproj", "Doerly.Module.Statistics.Api/"]
COPY ["Doerly.Module.Statistics.DataTransferObjects/Doerly.Module.Statistics.DataTransferObjects.csproj", "Doerly.Module.Statistics.DataTransferObjects/"]
COPY ["Doerly.Module.Statistics.Domain/Doerly.Module.Statistics.Domain.csproj", "Doerly.Module.Statistics.Domain/"]
RUN dotnet restore "./Doerly.Host/Doerly.Host.csproj"
COPY . .
WORKDIR "/src/Doerly.Host"
RUN dotnet build "./Doerly.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Doerly.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false -m:1

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Doerly.Host.dll"]