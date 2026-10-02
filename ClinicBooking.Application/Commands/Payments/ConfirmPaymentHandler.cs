using MediatR;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Verifierar token‑strängen från länken, markerar fakturan som betald
/// och gör länken engångsanvänd (hashen tas bort).
/// </summary>
public sealed class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand>
{
    private readonly IPaymentRepository _paymentRepository;

    public ConfirmPaymentHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task Handle(ConfirmPaymentCommand request,
                             CancellationToken cancellationToken)
    {
        // -------------------------------------------------
        // 1. Hasha token exakt som i SendPaymentLinkHandler
        //    (SHA‑256 av token‑strängen, hex‑representation)
        // -------------------------------------------------
        string hash = Convert.ToHexString(
                          SHA256.HashData(Encoding.UTF8.GetBytes(request.Token)));

        // -------------------------------------------------
        // 2. Hämta fakturan via hash‑värdet
        // -------------------------------------------------
        var payment = await _paymentRepository
            .GetByVerificationHashAsync(hash);

        if (payment == null)
            throw new Exception("ogiltig länk");

        // -------------------------------------------------
        // 3. Kontrollera att länken inte har gått ut (max 48 h)
        // -------------------------------------------------
        if (!payment.VerificationSentAt.HasValue ||
            DateTime.UtcNow - payment.VerificationSentAt.Value > TimeSpan.FromHours(48))
        {
            throw new Exception("ogiltig länk");
        }

        // -------------------------------------------------
        // 4. Markera fakturan som betald
        // -------------------------------------------------
        payment.Status = PaymentStatus.Paid;
        payment.PaidAt = DateTime.UtcNow;
        payment.VerifiedAt = DateTime.UtcNow;
        payment.VerificationCodeHash = null;   // länken blir engångs‑länk

        // -------------------------------------------------
        // 5. Spara ändringarna
        // -------------------------------------------------
        await _paymentRepository.UpdateAsync(payment);
    }
}
