# FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
# WORKDIR /app
# EXPOSE 5000
# FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
# WORKDIR /src
# COPY ["DUTPS.API/DUTPS.API.csproj", "DUTPS.API/"]
# COPY ["DUTPS.Commons/DUTPS.Commons.csproj", "DUTPS.Commons/"]
# COPY ["DUTPS.Databases/DUTPS.Databases.csproj", "DUTPS.Databases/"]
# RUN dotnet restore "DUTPS.API/DUTPS.API.csproj"
# COPY . .
# WORKDIR "/src/DUTPS.API"
# RUN dotnet build "DUTPS.API.csproj" -c Release -o /app/build
# FROM build AS publish
# RUN dotnet publish "DUTPS.API.csproj" -c Release -o /app/publish
# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "DUTPS.API.dll"]
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
EXPOSE 80
WORKDIR /src
COPY ["DUTPS.API/DUTPS.API.csproj", "DUTPS.API/"]
COPY ["DUTPS.Commons/DUTPS.Commons.csproj", "DUTPS.Commons/"]
COPY ["DUTPS.Databases/DUTPS.Databases.csproj", "DUTPS.Databases/"]

RUN dotnet restore "DUTPS.API/DUTPS.API.csproj" \
    && dotnet tool install --global dotnet-ef --version 6.0.0
COPY . .

FROM build AS development
WORKDIR /src/DUTPS.API
CMD dotnet watch run --urls http://0.0.0.0:5000

FROM build AS production
WORKDIR /src/DUTPS.API
RUN dotnet publish "DUTPS.API.csproj" -c Release -o /src/publish
WORKDIR /src/publish
ENTRYPOINT ["dotnet", "DUTPS.API.dll", "--urls", "http://0.0.0.0:80"]
