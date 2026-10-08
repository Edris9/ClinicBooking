using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MediatR;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
namespace ClinicBooking.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ---------------------------  Grundläggande services  ---------------------------
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ---------------------------  MediatR & AutoMapper ---------------------------
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(
                typeof(BookAppointmentHandler).Assembly));

            builder.Services.AddAutoMapper(cfg => cfg.AddMaps(
                typeof(MappingProfile).Assembly));

            // ---------------------------  EF‑Core ---------------------------------------
            builder.Services.AddDbContext<ClinicDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // ---------------------------  Repositories ---------------------------------
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddHttpClient<IEmailService, NotificationClient>(client => client.BaseAddress = new Uri(builder.Configuration["Notifications:BaseUrl"]!));

            // ---------------------------  JWT -----------------------------------------
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(
                                builder.Configuration["JwtSettings:SecretKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            // ---------------------------  CORS ----------------------------------------
            // Adressen för Blazor‑klienten läses från konfigurationen.
            var clientOrigin = builder.Configuration["ClientBaseUrl"]
                               ?? "https://localhost:7172";   // fallback‑värde

            // (…) i ClinicBooking.API/Program.cs, precis innan app.Build()
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://13.53.186.35:7172")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Kör migreringar automatiskt vid start (skapar databasen i Docker)
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ClinicDbContext>();
                db.Database.Migrate();
                if (!db.Departments.Any())
                {
                    db.Departments.AddRange(
                            // Akut & Internmedicin
                            new() { Name = "Akutmedicin", Description = "Akutsjukvård, triage och omedelbart omhändertagande." },
                            new() { Name = "Allmänmedicin", Description = "Primärvård och allmänläkarmottagning." },
                            new() { Name = "Kardiologi", Description = "Utredning och behandling av hjärtsjukdomar." },
                            new() { Name = "Neurologi", Description = "Sjukdomar i hjärnan och nervsystemet." },
                            new() { Name = "Pulmonologi", Description = "Lungsjukdomar, astma och andningsbesvär." },
                            new() { Name = "Gastroenterologi", Description = "Sjukdomar i mage, tarmar och leversystem." },

                            // Kirurgi & Ortopedi
                            new() { Name = "Allmänkirurgi", Description = "Operativa ingrepp i buken och mjukdelar." },
                            new() { Name = "Ortopedi", Description = "Sjukdomar och skador i skelett, leder och muskler." },
                            new() { Name = "Urologi", Description = "Sjukdomar i urinvägarna och manliga könsorgan." },
                            new() { Name = "Plastikkirurgi", Description = "Rekonstruktiv kirurgi efter skador eller tumörer." },

                            // Kvinnor & Barn
                            new() { Name = "Pediatrik", Description = "Specialiserad barn- och ungdomsmedicin." },
                            new() { Name = "Gynekologi", Description = "Kvinnosjukvård och reproduktiv hälsa." },

                            // Specialiteter
                            new() { Name = "Dermatologi", Description = "Utredning och behandling av hudåkommor." },
                            new() { Name = "Onkologi", Description = "Medicinsk cancerbehandling och tumörsjukdomar." },
                            new() { Name = "Psykiatri", Description = "Specialiserad psykiatrisk vård och samtalsstöd." },
                            new() { Name = "Geriatrik", Description = "Specialiserad vård och rehabilitering för äldre." }
                        );
                    db.SaveChanges();
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("BlazorClient");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseCors("AllowFrontend");

            app.Run();
        }
    }
}
