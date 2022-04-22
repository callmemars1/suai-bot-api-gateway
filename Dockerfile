#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["suai-api-schedule/suai-api-schedule.csproj", "suai-api-schedule/"]
RUN dotnet restore "suai-api-schedule/suai-api-schedule.csproj"
COPY . .
WORKDIR "/src/suai-api-schedule"
RUN dotnet build "suai-api-schedule.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "suai-api-schedule.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "suai-api-schedule.dll"]