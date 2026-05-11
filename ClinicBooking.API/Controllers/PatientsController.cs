using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;



[ApiController]
[Route("api/[controller]")]

public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;
    public PatientsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreatePatientCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        {
        var result = await _mediator.Send(new GetPatientByIdQuery { Id = id });
        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllPatientsQuery());
        return Ok(result);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePatientCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }
        await _mediator.Send(command);
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeletePatientCommand { Id = id });
        return NoContent();
    }
}