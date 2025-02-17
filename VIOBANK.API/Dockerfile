# Используем официальный образ .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем файлы проекта и решение
COPY VIOBANK.API/appsettings.json /app/appsettings.json
COPY VIOBANK.API/appsettings.Development.json /app/appsettings.Development.json


#COPY ["VIOBANK.sln", "./"]
#COPY ["VIOBANK.API/VIOBANK.API.csproj", "VIOBANK.API/"]
#COPY ["VIOBANK.Application/VIOBANK.Application.csproj", "VIOBANK.Application/"]
#COPY ["VIOBANK.Domain/VIOBANK.Domain.csproj", "VIOBANK.Domain/"]
#COPY ["VIOBANK.Infrastructure/VIOBANK.Infrastructure.csproj", "VIOBANK.Infrastructure/"]
#COPY ["VIOBANK.PostgresPersistence/VIOBANK.PostgresPersistence.csproj", "VIOBANK.PostgresPersistence/"]
#COPY ["VIOBANK.RedisPersistence/VIOBANK.RedisPersistence.csproj", "VIOBANK.RedisPersistence/"]

COPY ./VIOBANK.API/appsettings.json /app/appsettings.json
COPY ./VIOBANK.API/appsettings.Development.json /app/appsettings.Development.json
COPY ./VIOBANK.sln ./
COPY ./VIOBANK.API/VIOBANK.API.csproj ./VIOBANK.API/
COPY ./VIOBANK.Application/VIOBANK.Application.csproj ./VIOBANK.Application/
COPY ./VIOBANK.Domain/VIOBANK.Domain.csproj ./VIOBANK.Domain/
COPY ./VIOBANK.Infrastructure/VIOBANK.Infrastructure.csproj ./VIOBANK.Infrastructure/
COPY ./VIOBANK.PostgresPersistence/VIOBANK.PostgresPersistence.csproj ./VIOBANK.PostgresPersistence/
COPY ./VIOBANK.RedisPersistence/VIOBANK.RedisPersistence.csproj ./VIOBANK.RedisPersistence/

# Восстанавливаем зависимости
RUN dotnet restore

# Копируем весь код
COPY . .

# Собираем проект
RUN dotnet publish "VIOBANK.API/VIOBANK.API.csproj" -c Release -o /out

# Используем легковесный образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

# Запускаем API
ENTRYPOINT ["dotnet", "VIOBANK.API.dll"]
