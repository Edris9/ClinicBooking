# ----- Build (Blazor WebAssembly) -----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ClinicBooking.Client/ClinicBooking.Client.csproj", "ClinicBooking.Client/"]
RUN dotnet restore "ClinicBooking.Client/ClinicBooking.Client.csproj"

COPY . .
WORKDIR "/src/ClinicBooking.Client"
RUN dotnet publish "ClinicBooking.Client.csproj" -c Release -o /app/publish

# ----- Runtime – nginx (serve static files) -----
FROM nginx:stable-alpine AS runtime

# statiska blazor‑filer
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

# <--  ÄNDRA den här raden  -->
COPY ClinicBooking.Client/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]
