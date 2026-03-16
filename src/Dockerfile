FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=release
WORKDIR /src

COPY ["atividade-4.1/atividade-4.1.csproj", "atividade-4.1/"]
RUN dotnet restore "./atividade-4.1/atividade-4.1.csproj"

COPY . .

RUN dotnet build "atividade-4.1/atividade-4.1.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=release
RUN dotnet publish "atividade-4.1/atividade-4.1.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "atividade-4.1.dll"]