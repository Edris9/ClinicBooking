// SendPaymentLinkHandlerTests.cs
// ---------------------------------------------------------------
//  Unit‑test för SendPaymentLinkHandler
//  - verifierar att e‑post skickas med korrekt länk
//  - verifierar att fakturan uppdateras (hash & timestamp)
//  - kontrollerar fel‑beteenden (saknad faktura, redan betald)
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

// Gränssnitten finns i ditt projekt – ingen extra using‑namespace behövs
// (de är i projekt‑roten utan namnrymd)


public class SendPaymentLinkHandlerTests
{
    // -----------------------------------------------------------
    //  Hjälp‑metod: bygger en enkel token‑hash‑parning för verifiering
    // -----------------------------------------------------------
    private static string ComputeHash(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

    // -----------------------------------------------------------
    //  Test: lyckad sändning av betalningslänk
    // -----------------------------------------------------------
    [Fact]
    public async Task Handle_SendsEmail_UpdatesPayment_WithHashAndTimestamp()
    {
        // ----- Arrange -------------------------------------------------
        const int paymentId = 42;
        const int appointmentId = 7;
        const int patientId = 3;

        var payment = new Payment
        {
            Id = paymentId,
            Amount = 123.45m,
            DueDate = DateTime.Today.AddDays(5),
            Status = PaymentStatus.Unpaid,
            AppointmentId = appointmentId
        };

        var appointment = new Appointment
        {
            Id = appointmentId,
            PatientId = patientId
        };

        var patient = new Patient
        {
            Id = patientId,
            Email = "Edriskohestani1010@gmail.com",
            Name = "Testpatient"
        };

        // Mockar repos
        var paymentRepo = new Mock<IPaymentRepository>();
        var appointRepo = new Mock<IAppointmentRepository>();
        var patientRepo = new Mock<IPatientRepository>();
        var emailService = new Mock<IEmailService>();

        paymentRepo.Setup(r => r.GetByIdAsync(paymentId))
                   .ReturnsAsync(payment);
        appointRepo.Setup(r => r.GetByIdAsync(appointmentId))
                   .ReturnsAsync(appointment);
        patientRepo.Setup(r => r.GetByIdAsync(patientId))
                   .ReturnsAsync(patient);

        // fånga argumenten som skickas till UpdateAsync så vi kan inspektera dem
        Payment? updatedPayment = null;
        paymentRepo.Setup(r => r.UpdateAsync(It.IsAny<Payment>()))
                   .Callback<Payment>(p => updatedPayment = p)
                   .Returns(Task.CompletedTask)
                   .Verifiable();

        // fånga argumenten till e‑post‑metoden
        string? mailTo = null;
        string? mailSubject = null;
        string? mailBody = null;

        // ny kod – anger alla parametrar (det fjärde har ingen default i testet)
        emailService.Setup(s => s.SendEmailAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()))

                            .Callback<string, string, string, CancellationToken>((to, sub, body, ct) =>
                            {
                        mailTo = to;
                        mailSubject = sub;
                        mailBody = body;
                    })
                    .Returns(Task.CompletedTask)
                    .Verifiable();

        var handler = new SendPaymentLinkHandler(
            paymentRepo.Object,
            appointRepo.Object,
            patientRepo.Object,
            emailService.Object);

        var command = new SendPaymentLinkCommand(paymentId, "https://myapi.test");

        // ----- Act ----------------------------------------------------
        await handler.Handle(command, CancellationToken.None);

        // ----- Assert -------------------------------------------------
        // 1. E‑post skickades till rätt mottagare
        Assert.Equal(patient.Email, mailTo);
        Assert.NotNull(mailSubject);
        Assert.NotNull(mailBody);

        // 2. Länken finns i mail‑kroppen och innehåller token‑parametern
        const string tokenMarker = "token=";
        var tokenStart = mailBody!.IndexOf(tokenMarker, StringComparison.Ordinal);
        Assert.True(tokenStart >= 0, "Token‑parameter saknas i e‑postens body.");
        var token = mailBody.Substring(tokenStart + tokenMarker.Length)
                           .Split(new[] { '&', '"', ' ', '<' }, StringSplitOptions.RemoveEmptyEntries)[0];
        Assert.False(string.IsNullOrWhiteSpace(token), "Token‑värde är tomt.");

        // 3. Fakturan uppdaterades (VerificationCodeHash + VerificationSentAt)
        Assert.NotNull(updatedPayment);
        Assert.NotNull(updatedPayment!.VerificationCodeHash);
        Assert.NotNull(updatedPayment.VerificationSentAt);

        // 4. Hashen som sparas motsvarar token‑hashen som definierats i handlern
        var expectedHash = ComputeHash(token);
        Assert.Equal(expectedHash, updatedPayment.VerificationCodeHash,
                     ignoreCase: true);

        // 5. Verify att alla mocks faktiskt anropades
        paymentRepo.Verify(r => r.GetByIdAsync(paymentId), Times.Once);
        appointRepo.Verify(r => r.GetByIdAsync(appointmentId), Times.Once);
        patientRepo.Verify(r => r.GetByIdAsync(patientId), Times.Once);
        paymentRepo.Verify(r => r.UpdateAsync(It.IsAny<Payment>()), Times.Once);
        emailService.Verify(s => s.SendEmailAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    // -----------------------------------------------------------
    //  Test: faktura finns ej → Exception
    // -----------------------------------------------------------
    [Fact]
    public async Task Handle_Throws_WhenPaymentNotFound()
    {
        var paymentRepo = new Mock<IPaymentRepository>();
        var appointRepo = new Mock<IAppointmentRepository>();
        var patientRepo = new Mock<IPatientRepository>();
        var emailService = new Mock<IEmailService>();

        paymentRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((Payment?)null);   // tydligt nullable

        var handler = new SendPaymentLinkHandler(
            paymentRepo.Object,
            appointRepo.Object,
            patientRepo.Object,
            emailService.Object);

        var command = new SendPaymentLinkCommand(999, "https://api");

        var ex = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        Assert.Contains("Payment not found", ex.Message);
    }

    // -----------------------------------------------------------
    //  Test: fakturan är redan betald → Exception
    // -----------------------------------------------------------
    [Fact]
    public async Task Handle_Throws_WhenPaymentAlreadyPaid()
    {
        var payment = new Payment
        {
            Id = 1,
            Status = PaymentStatus.Paid,   // redan betald
            AppointmentId = 1
        };

        var paymentRepo = new Mock<IPaymentRepository>();
        var appointRepo = new Mock<IAppointmentRepository>();
        var patientRepo = new Mock<IPatientRepository>();
        var emailService = new Mock<IEmailService>();

        paymentRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                   .ReturnsAsync(payment);

        var handler = new SendPaymentLinkHandler(
            paymentRepo.Object,
            appointRepo.Object,
            patientRepo.Object,
            emailService.Object);

        var command = new SendPaymentLinkCommand(1, "https://api");

        var ex = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        Assert.Contains("already paid", ex.Message);
    }

    // -----------------------------------------------------------
    //  Test: patient saknar e‑post → Exception
    // -----------------------------------------------------------
    [Fact]
    public async Task Handle_Throws_WhenPatientHasNoEmail()
    {
        var payment = new Payment
        {
            Id = 2,
            Status = PaymentStatus.Unpaid,
            AppointmentId = 2
        };

        var appointment = new Appointment { Id = 2, PatientId = 3 };
        var patient = new Patient { Id = 3, Email = null! }; // ingen e‑post

        var paymentRepo = new Mock<IPaymentRepository>();
        var appointRepo = new Mock<IAppointmentRepository>();
        var patientRepo = new Mock<IPatientRepository>();
        var emailService = new Mock<IEmailService>();

        paymentRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(payment);
        appointRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(appointment);
        patientRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(patient);

        var handler = new SendPaymentLinkHandler(
            paymentRepo.Object,
            appointRepo.Object,
            patientRepo.Object,
            emailService.Object);

        var command = new SendPaymentLinkCommand(2, "https://api");

        var ex = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        Assert.Contains("no e-mail address", ex.Message);
    }
}
