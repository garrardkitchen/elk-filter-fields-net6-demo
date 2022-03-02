FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY Logstash.Web.csproj .
RUN dotnet restore Logstash.Web.csproj
COPY . .
WORKDIR /src/
RUN dotnet build Logstash.Web.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Logstash.Web.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ENV ASPNETCORE_ENVIRONMENT="Develoment"
# ENV ASPNETCORE_URLS=http://+:5005
EXPOSE 5005

ENTRYPOINT ["dotnet", "Logstash.Web.dll"]
