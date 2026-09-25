using Moq; // Krävs för att du ska kunna använda Mock<T>
using Xunit; // Krävs för [Fact] och Assert
using ClinicBooking.Application.Commands; // Ändra till det namespace där din Handler ligger
using ClinicBooking.Core.Interfaces;     // Ändra till ditt namespace för Interfaces
using ClinicBooking.Core.Entities;       // Ändra till ditt namespace för Entities

namespace ClinicBooking.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task SkapaBokning_SkaLyckas_NärDataÄrGiltig()
        {
            // 1. ARRANGE (Förberedelse och Mockning)
            // Här skapar vi låtsasversioner (mocks) av dina databaslager
            var appointmentRepoMock = new Mock<IAppointmentRepository>();
            var patientRepoMock = new Mock<IPatientRepository>();
            var doctorRepoMock = new Mock<IDoctorRepository>();

            // Vi ställer in att om handlern söker efter en Patient eller Läkare, 
            // så ska våra mocks returnera ett giltigt objekt istället för null (databas behövs ej!)
            patientRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(new Patient());

            doctorRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                          .ReturnsAsync(new Doctor());

            // Skapa själva handlern och skicka in dina tre mock-objekt (.Object)
            var handler = new BookAppointmentHandler(
                appointmentRepoMock.Object,
                patientRepoMock.Object,
                doctorRepoMock.Object
            );

            // Skapa ett kommando med testdata (ID:n för patient och läkare)
            var command = new BookAppointmentCommand
            {
                PatientId = Guid.NewGuid(),
                DoctorId = Guid.NewGuid(),
                AppointmentDate = DateTime.Now.AddDays(2)
            };

            // 2. ACT (Utför handlingen)
            // Här anropar vi metoden i din BookAppointmentHandler
            var resultat = await handler.Handle(command, CancellationToken.None);

            // 3. ASSERT (Kontrollera resultatet)
            // Här kontrollerar vi att allt gick som förväntat
            Assert.NotNull(resultat);

            // Kontrollera att din handler faktiskt försökte spara bokningen i databasen en gång (Times.Once)
            appointmentRepoMock.Verify(repo => repo.AddAsync(It.IsAny<Appointment>()), Times.Once);
        }
    }
}
