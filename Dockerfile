# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copy từng csproj cho restore
COPY src/Application/NorthwindApi.Application/NorthwindApi.Application.csproj src/Application/NorthwindApi.Application/
COPY src/Domain/NorthwindApi.Domain/NorthwindApi.Domain.csproj src/Domain/NorthwindApi.Domain/
COPY src/Infrastructure/NorthwindApi.Infrastructure/NorthwindApi.Infrastructure.csproj src/Infrastructure/NorthwindApi.Infrastructure/
COPY src/Infrastructure/NorthwindApi.Persistence/NorthwindApi.Persistence.csproj src/Infrastructure/NorthwindApi.Persistence/
COPY src/Presentation/Api/NorthwindApi.Api/NorthwindApi.Api.csproj src/Presentation/Api/NorthwindApi.Api/

# Restore
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore "src/Presentation/Api/NorthwindApi.Api/NorthwindApi.Api.csproj"

# Copy toàn bộ source
COPY . .

# Publish
WORKDIR /src/src/Presentation/Api/NorthwindApi.Api
RUN dotnet publish "NorthwindApi.Api.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

RUN apk add --no-cache tzdata icu-libs \
    && cp /usr/share/zoneinfo/Asia/Ho_Chi_Minh /etc/localtime \
    && echo "Asia/Ho_Chi_Minh" > /etc/timezone

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build /app .

EXPOSE 8080

USER $APP_UID
ENTRYPOINT ["dotnet", "NorthwindApi.Api.dll"]