FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/Shared/Infrastructure/Api/Api.csproj", "src/Shared/Infrastructure/Api/"]
COPY ["src/Module/Order/Order.csproj", "src/Module/Order/"]
RUN dotnet restore "src/Shared/Infrastructure/Api/Api.csproj"

COPY . .
WORKDIR "/src/src/Shared/Infrastructure/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Api.dll"]
