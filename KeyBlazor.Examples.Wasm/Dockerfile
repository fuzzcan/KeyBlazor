FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["KeyBlazor.Examples.Wasm/KeyBlazor.Examples.Wasm.csproj", "KeyBlazor.Examples.Wasm/"]
RUN dotnet restore "KeyBlazor.Examples.Wasm/KeyBlazor.Examples.Wasm.csproj"
COPY . .
WORKDIR "/src/KeyBlazor.Examples.Wasm"
RUN dotnet build "KeyBlazor.Examples.Wasm.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "KeyBlazor.Examples.Wasm.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KeyBlazor.Examples.Wasm.dll"]
