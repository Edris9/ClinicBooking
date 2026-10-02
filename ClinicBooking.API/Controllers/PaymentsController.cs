using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _config;

    public PaymentsController(IMediator mediator, IConfiguration config)
    {
        _mediator = mediator;
        _config = config;
    }

    // -----------------------------------------------------------------
    // Befintliga endpoints (oförändrade)
    // -----------------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllPaymentsQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePaymentCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePaymentCommand command)
    {
        if (id != command.Id) return BadRequest();
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeletePaymentCommand { Id = id });
        return NoContent();
    }

    // -----------------------------------------------------------------
    // 1️⃣  POST  {id}/send-link
    // -----------------------------------------------------------------
    /// <summary>
    /// Skickar ett e‑postmeddelande med en engångslänk för betalning av fakturan.
    /// Base‑URL läses från appsettings (t.ex. "AppSettings:BaseUrl").
    /// </summary>
    [HttpPost("{id}/send-link")]
    public async Task<IActionResult> SendLink(int id)
    {
        // Hämtar den konfigurerade API‑bas‑URL:en.
        // Nyckeln du använder i appsettings kan variera – anpassa vid behov.
        var baseUrl = _config.GetValue<string>("AppSettings:BaseUrl")
                     ?? $"{Request.Scheme}://{Request.Host}";

        await _mediator.Send(new SendPaymentLinkCommand(id, baseUrl));
        return NoContent();                     // 204 – ingen body behövs
    }

    // -----------------------------------------------------------------
    // 2️⃣  GET  confirm?token=...
    // -----------------------------------------------------------------
    /// <summary>
    /// Mottar token‑parametern från länken, bekräftar betalningen och
    /// omdirigerar sedan användaren till Blazor‑sidan “payment‑confirmed”.
    /// </summary>
    [HttpGet("confirm")]
    public async Task<IActionResult> Confirm([FromQuery] string token)
    {
        var clientUrl = _config["AppSettings:ClientBaseUrl"] ?? "https://localhost:7172";

        try
        {
            await _mediator.Send(new ConfirmPaymentCommand(token));
            return Redirect($"{clientUrl}/payment-confirmed?status=ok");
        }
        catch
        {
            return Redirect($"{clientUrl}/payment-confirmed?status=invalid");
        }
    }
}
