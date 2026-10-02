using System.Security.Cryptography;
using System.Text;
using MediatR;

public class SendPaymentLinkHandler : IRequestHandler<SendPaymentLinkCommand>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IEmailService _emailService;

    public SendPaymentLinkHandler(
        IPaymentRepository paymentRepository,
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository,
        IEmailService emailService)
    {
        _paymentRepository = paymentRepository;
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _emailService = emailService;
    }

    public async Task Handle(SendPaymentLinkCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId)
            ?? throw new Exception("Payment not found");

        if (payment.Status == PaymentStatus.Paid)
            throw new Exception("Payment is already paid");

        var appointment = await _appointmentRepository.GetByIdAsync(payment.AppointmentId)
            ?? throw new Exception("Appointment not found");

        var patient = await _patientRepository.GetByIdAsync(appointment.PatientId)
            ?? throw new Exception("Patient not found");

        if (string.IsNullOrWhiteSpace(patient.Email))
            throw new Exception("Patient has no e-mail address");

        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

        payment.VerificationCodeHash = hash;
        payment.VerificationSentAt = DateTime.UtcNow;
        await _paymentRepository.UpdateAsync(payment);

        var link = $"{request.BaseUrl.TrimEnd('/')}/api/payments/confirm?token={token}";

        var body = $@"
            <p>Hej {patient.Name},</p>
            <p>Du har en faktura på <strong>{payment.Amount:N2} kr</strong> som förfaller {payment.DueDate:yyyy-MM-dd}.</p>
            <p><a href=""{link}"">Bekräfta betalning</a></p>
            <p>Länken gäller i 48 timmar.</p>";

        await _emailService.SendEmailAsync(patient.Email, "Din faktura från kliniken", body);
    }
}