using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


// Lyckat — patient och doktor finns, appointment skapas
//Misslyckat — patient finns inte, exception kastas


public class BookAppointmentHandlerTests
{
    private readonly Mock<IAppointmentRepository> _appointmentRepo;
    private readonly Mock<IPatientRepository> _patientRepo;
    private readonly Mock<IDoctorRepository> _doctorRepo;
    private readonly BookAppointmentHandler _handler;

    public BookAppointmentHandlerTests()
    {
        _appointmentRepo = new Mock<IAppointmentRepository>();
        _patientRepo = new Mock<IPatientRepository>();
        _doctorRepo = new Mock<IDoctorRepository>();

        _handler = new BookAppointmentHandler(
            _appointmentRepo.Object,
            _patientRepo.Object,
            _doctorRepo.Object
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsAppointmentId()
    {
        // Arrange
        _patientRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Patient { Id = 1 });
        _doctorRepo.Setup(x => x.GetByIdAsync(5)).ReturnsAsync(new Doctor { Id = 5 });
        _appointmentRepo.Setup(x => x.AddAsync(It.IsAny<Appointment>())).Returns(Task.CompletedTask);

        var command = new BookAppointmentCommand
        {
            PatientId = 1,
            DoctorId = 5,
            Reason = "hosta"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(0, result); // Id är 0 eftersom ingen riktig databas
        _appointmentRepo.Verify(x => x.AddAsync(It.IsAny<Appointment>()), Times.Once);
    }

    [Fact]
    public async Task Handle_PatientNotFound_ThrowsException()
    {
        // Arrange
        _patientRepo.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Patient)null);

        var command = new BookAppointmentCommand
        {
            PatientId = 99,
            DoctorId = 5,
            Reason = "hosta"
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}