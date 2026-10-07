# -------------------------------------------------
#  1️⃣  Build‑steg – använder .NET SDK
# -------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

#  Kopiera bara csproj‑filen först (snabbare cache)
COPY ["ClinicBooking.Notifications/ClinicBooking.Notifications.csproj", 
      "ClinicBooking.Notifications/"]
RUN dotnet restore "ClinicBooking.Notifications/ClinicBooking.Notifications.csproj"

#  Kopiera resten av källkoden
COPY . .
WORKDIR "/src/ClinicBooking.Notifications"

#  Bygg och publicera i Release‑läge
RUN dotnet publish "ClinicBooking.Notifications.csproj" \
    -c Release -o /app/publish --no-restore

# -------------------------------------------------
#  2️⃣  Runtime‑steg – kör med den minsta ASP.NET‑imagen
# -------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

#  Ta över den publicerade outputen från bygg‑steg
COPY --from=build /app/publish .

#  Exponera HTTP‑port (Docker‑compose mappar den till en host‑port)
EXPOSE 80

#  Starta API‑t
ENTRYPOINT ["dotnet", "ClinicBooking.Notifications.dll"]
