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

            // --------------------------- Grundläggande services ---------------------------
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // --------------------------- MediatR & AutoMapper ---------------------------
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(
                    typeof(BookAppointmentHandler).Assembly));

            builder.Services.AddAutoMapper(cfg =>
                cfg.AddMaps(typeof(MappingProfile).Assembly));

            // --------------------------- EF Core -----------------------------------------
            builder.Services.AddDbContext<ClinicDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // --------------------------- Repositories ------------------------------------
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

            builder.Services.AddHttpClient<IEmailService, NotificationClient>(
                client => client.BaseAddress = new Uri(
                    builder.Configuration["Notifications:BaseUrl"]!));

            // --------------------------- JWT ---------------------------------------------
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

            builder.Services.AddAuthentication(
                    JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.ASCII.GetBytes(
                                    builder.Configuration["JwtSettings:SecretKey"]!)),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                });

            // --------------------------- CORS --------------------------------------------
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(
                            "https://localhost:7172",
                            "http://localhost:7172",
                            "http://13.53.186.35:7172")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // --------------------------- Database migration & seeding --------------------
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider
                    .GetRequiredService<ClinicDbContext>();

                db.Database.Migrate();

                var departments = new[]
                {
                    new Department
                    {
                        Name = "Akutmedicin",
                        Description = "Akutsjukvård, triage och omedelbart omhändertagande."
                    },
                    new Department
                    {
                        Name = "Allmänmedicin",
                        Description = "Primärvård och allmänläkarmottagning."
                    },
                    new Department
                    {
                        Name = "Kardiologi",
                        Description = "Utredning och behandling av hjärtsjukdomar."
                    },
                    new Department
                    {
                        Name = "Neurologi",
                        Description = "Sjukdomar i hjärnan och nervsystemet."
                    },
                    new Department
                    {
                        Name = "Pulmonologi",
                        Description = "Lungsjukdomar, astma och andningsbesvär."
                    },
                    new Department
                    {
                        Name = "Gastroenterologi",
                        Description = "Sjukdomar i mage, tarmar och leversystem."
                    },
                    new Department
                    {
                        Name = "Allmänkirurgi",
                        Description = "Operativa ingrepp i buken och mjukdelar."
                    },
                    new Department
                    {
                        Name = "Ortopedi",
                        Description = "Sjukdomar och skador i skelett, leder och muskler."
                    },
                    new Department
                    {
                        Name = "Urologi",
                        Description = "Sjukdomar i urinvägarna."
                    },
                    new Department
                    {
                        Name = "Plastikkirurgi",
                        Description = "Rekonstruktiv kirurgi."
                    },
                    new Department
                    {
                        Name = "Pediatrik",
                        Description = "Specialiserad barn- och ungdomsmedicin."
                    },
                    new Department
                    {
                        Name = "Gynekologi",
                        Description = "Kvinnosjukvård och reproduktiv hälsa."
                    },
                    new Department
                    {
                        Name = "Dermatologi",
                        Description = "Utredning och behandling av hudåkommor."
                    },
                    new Department
                    {
                        Name = "Onkologi",
                        Description = "Medicinsk cancerbehandling och tumörsjukdomar."
                    },
                    new Department
                    {
                        Name = "Psykiatri",
                        Description = "Specialiserad psykiatrisk vård och samtalsstöd."
                    },
                    new Department
                    {
                        Name = "Geriatrik",
                        Description = "Specialiserad vård och rehabilitering för äldre."
                    }
                };

                var existingDepartments = db.Departments.ToList();

                // Uppdatera de två befintliga posterna utan att ändra deras ID.
                var department1 = existingDepartments
                    .FirstOrDefault(d => d.Id == 1);

                if (department1 != null)
                {
                    department1.Name = "Kardiologi";
                    department1.Description =
                        "Utredning och behandling av hjärtsjukdomar.";
                }

                var department2 = existingDepartments
                    .FirstOrDefault(d => d.Id == 2);

                if (department2 != null)
                {
                    department2.Name = "Akutmedicin";
                    department2.Description =
                        "Akutsjukvård, triage och omedelbart omhändertagande.";
                }

                // Lägg bara till avdelningar som saknas.
                var existingNames = existingDepartments
                    .Select(d => d.Name)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (var department in departments)
                {
                    if (existingNames.Add(department.Name))
                    {
                        db.Departments.Add(department);
                    }
                }

                db.SaveChanges();
                Console.WriteLine("=== DEPARTMENTS AFTER SEEDING ===");

                foreach (var department in db.Departments.ToList())
                {
                    Console.WriteLine($"{department.Id}: {department.Name}");
                }
            }

            // --------------------------- Middleware -------------------------------------
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

