using Moq;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class ConfirmPaymentHandlerTests
{
    private static string Hash(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

    [Fact]
    public async Task Handle_ValidToken_MarksPaymentAsPaid()
    {
        // Arrange
        var token = "ABC123";
        var payment = new Payment
        {
            Id = 1,
            Status = PaymentStatus.Unpaid,
            VerificationCodeHash = Hash(token),
            VerificationSentAt = DateTime.UtcNow.AddHours(-1)
        };
        var repo = new Mock<IPaymentRepository>();
        repo.Setup(r => r.GetByVerificationHashAsync(Hash(token))).ReturnsAsync(payment);
        var handler = new ConfirmPaymentHandler(repo.Object);

        // Act
        await handler.Handle(new ConfirmPaymentCommand(token), CancellationToken.None);

        // Assert
        Assert.Equal(PaymentStatus.Paid, payment.Status);
        Assert.NotNull(payment.PaidAt);
        Assert.Null(payment.VerificationCodeHash);
        repo.Verify(r => r.UpdateAsync(payment), Times.Once);
    }

    [Fact]
    public async Task Handle_UnknownToken_Throws()
    {
        var repo = new Mock<IPaymentRepository>();
        var handler = new ConfirmPaymentHandler(repo.Object);

        await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(new ConfirmPaymentCommand("FEL"), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ExpiredToken_ThrowsAndStaysUnpaid()
    {
        var token = "ABC123";
        var payment = new Payment
        {
            Status = PaymentStatus.Unpaid,
            VerificationCodeHash = Hash(token),
            VerificationSentAt = DateTime.UtcNow.AddHours(-49)
        };
        var repo = new Mock<IPaymentRepository>();
        repo.Setup(r => r.GetByVerificationHashAsync(Hash(token))).ReturnsAsync(payment);
        var handler = new ConfirmPaymentHandler(repo.Object);

        await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(new ConfirmPaymentCommand(token), CancellationToken.None));

        Assert.Equal(PaymentStatus.Unpaid, payment.Status);
        repo.Verify(r => r.UpdateAsync(It.IsAny<Payment>()), Times.Never);
    }
}