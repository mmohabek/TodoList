# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TodoList.Api/TodoList.Api.csproj", "TodoList.Api/"]
COPY ["TodoList.Application/TodoList.Application.csproj", "TodoList.Application/"]
COPY ["TodoList.Domain/TodoList.Domain.csproj", "TodoList.Domain/"]
COPY ["TodoList.Infrastructure/TodoList.Infrastructure.csproj", "TodoList.Infrastructure/"]
RUN dotnet restore "TodoList.Api/TodoList.Api.csproj"
COPY . .
WORKDIR "/src/TodoList.Api"
RUN dotnet build "TodoList.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "TodoList.Api.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 80
ENTRYPOINT ["dotnet", "TodoList.Api.dll"]
