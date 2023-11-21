FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Mock.PersonInfo/Mock.PersonInfo.csproj", "Mock.PersonInfo/."]
RUN dotnet restore "Mock.PersonInfo/Mock.PersonInfo.csproj"
COPY . .
RUN dotnet build "Mock.PersonInfo/Mock.PersonInfo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mock.PersonInfo/Mock.PersonInfo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
COPY --from=publish /app/publish .
EXPOSE 8080

ENTRYPOINT ["dotnet", "Mock.PersonInfo.dll"]