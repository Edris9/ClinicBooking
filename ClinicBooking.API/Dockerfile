# ----- Build -----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj & restore (snabbast)
COPY ["ClinicBooking.API/ClinicBooking.API.csproj", "ClinicBooking.API/"]
COPY ["ClinicBooking.Application/ClinicBooking.Application.csproj", "ClinicBooking.Application/"]
COPY ["ClinicBooking.Domain/ClinicBooking.Domain.csproj", "ClinicBooking.Domain/"]
COPY ["ClinicBooking.Infrastructure/ClinicBooking.Infrastructure.csproj", "ClinicBooking.Infrastructure/"]
COPY ["ClinicBooking.Notifications/ClinicBooking.Notifications.csproj", "ClinicBooking.Notifications/"]

RUN dotnet restore "ClinicBooking.API/ClinicBooking.API.csproj"

# copy everything else & build
COPY . .
WORKDIR "/src/ClinicBooking.API"
RUN dotnet publish "ClinicBooking.API.csproj" -c Release -o /app --no-restore

# ----- Runtime -----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

# .NET 8 använder port 8080 som standard för okrypterad trafik
EXPOSE 8080

ENTRYPOINT ["dotnet", "ClinicBooking.API.dll"]
