FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CE-API-V2/CE-API-V2.csproj", "CE-API-V2/"]
RUN dotnet restore "CE-API-V2/CE-API-V2.csproj"
COPY . .
WORKDIR "/src/CE-API-V2"
RUN dotnet build "CE-API-V2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CE-API-V2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CE-API-V2.dll"]
