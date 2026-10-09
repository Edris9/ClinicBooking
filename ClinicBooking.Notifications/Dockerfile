# ----- Build -----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ClinicBooking.Notifications/ClinicBooking.Notifications.csproj", "ClinicBooking.Notifications/"]
RUN dotnet restore "ClinicBooking.Notifications/ClinicBooking.Notifications.csproj"

COPY . .
WORKDIR /src/ClinicBooking.Notifications
RUN dotnet publish "ClinicBooking.Notifications.csproj" -c Release -o /app/publish --no-restore

# ----- Runtime -----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
USER app
ENTRYPOINT ["dotnet", "ClinicBooking.Notifications.dll"]