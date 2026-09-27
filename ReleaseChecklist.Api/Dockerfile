FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ReleaseChecklist.Api/ReleaseChecklist.Api.csproj", "ReleaseChecklist.Api/"]
RUN dotnet restore "ReleaseChecklist.Api/ReleaseChecklist.Api.csproj"

COPY . .
WORKDIR "/src/ReleaseChecklist.Api"
RUN dotnet build "ReleaseChecklist.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReleaseChecklist.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReleaseChecklist.Api.dll"]