FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["src/MotorbikeRental/MotorbikeRental.sln", "MotorbikeRental/"]

COPY ["src/MotorbikeRental/MotorbikeRental.API/MotorbikeRental.API.csproj", "MotorbikeRental/MotorbikeRental.API/"]
COPY ["src/MotorbikeRental/MotorbikeRental.Application/MotorbikeRental.Application.csproj", "MotorbikeRental/MotorbikeRental.Application/"]
COPY ["src/MotorbikeRental/MotorbikeRental.Infrastructure/MotorbikeRental.Infrastructure.csproj", "MotorbikeRental/MotorbikeRental.Infrastructure/"]
COPY ["src/MotorbikeRental/MotorbikeRental.Common/MotorbikeRental.Common.csproj", "MotorbikeRental/MotorbikeRental.Common/"]
COPY ["src/MotorbikeRental/MotorbikeRental.Domain/MotorbikeRental.Domain.csproj", "MotorbikeRental/MotorbikeRental.Domain/"]

RUN dotnet restore "MotorbikeRental/MotorbikeRental.API/MotorbikeRental.API.csproj"

COPY ["src/MotorbikeRental/", "MotorbikeRental/"]

WORKDIR "/src/MotorbikeRental/MotorbikeRental.API"
RUN dotnet publish "MotorbikeRental.API.csproj" -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

RUN mkdir -p /app/uploads

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "MotorbikeRental.API.dll"]