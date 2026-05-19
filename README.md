# ClinicBooking




# ClinicBooking API

Ett ASP.NET Core Web API byggt med Clean Architecture för hantering av klinikbokningar.

## Tekniker

- **ASP.NET Core 8** — Web API
- **Clean Architecture** — 4 lager (Domain, Application, Infrastructure, API)
- **CQRS + MediatR** — Commands och Queries separerade
- **Entity Framework Core** — SQL Server databas
- **AutoMapper** — mappning mellan entiteter och DTOs
- **JWT Bearer** — autentisering och auktorisering
- **Swagger** — API-dokumentation

## Projektstruktur

```
ClinicBooking.Domain         # Entiteter, Interfaces, Enums
ClinicBooking.Application    # Commands, Queries, Handlers, DTOs
ClinicBooking.Infrastructure # DbContext, Repositories, JWT-tjänst
ClinicBooking.API            # Controllers, Program.cs
```

## Entiteter

- **Doctor** — tillhör en Department, har många Appointments
- **Patient** — har många Appointments
- **Appointment** — kopplar Doctor och Patient
- **Department** — har många Doctors
- **User** — för inloggning med roller (Admin, Doctor, Patient)

## Kom igång

```bash
git clone <repo-url>
cd ClinicBooking
dotnet restore
dotnet ef database update --project ClinicBooking.Infrastructure --startup-project ClinicBooking.API
dotnet run --project ClinicBooking.API
```

Öppna Swagger: `http://localhost:5277/swagger`

## Endpoints

| Metod | Endpoint | Beskrivning |
|-------|----------|-------------|
| POST | /api/Auth/Register | Skapa konto |
| POST | /api/Auth/login | Logga in, få JWT-token |
| POST | /api/Doctors | Skapa läkare |
| GET | /api/Doctors | Hämta alla läkare |
| POST | /api/Patients | Skapa patient |
| GET | /api/Patients | Hämta alla patienter |
| POST | /api/Appointments | Boka tid |
| GET | /api/Appointments/{id} | Hämta bokning |