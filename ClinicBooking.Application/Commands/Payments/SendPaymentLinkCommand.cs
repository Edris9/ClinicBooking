using MediatR;

/// <summary>
/// Command that triggers the creation and e‑mailing of a payment‑link.
/// </summary>
public sealed class SendPaymentLinkCommand : IRequest
{
    public int PaymentId { get; }
    public string BaseUrl { get; }

    public SendPaymentLinkCommand(int paymentId, string baseUrl)
    {
        PaymentId = paymentId;
        BaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
    }
}
