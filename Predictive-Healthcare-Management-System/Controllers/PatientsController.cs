using Application.Use_Cases.Commands;
using MediatR;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Application.Use_Cases.Queries;
using Domain.Utils;

namespace Predictive_Healthcare_Management_System.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreatePatient([FromBody] CreatePatientCommand command)
        {
            try
            {
                var result =  await _mediator.Send(command);
                return CreatedAtAction(nameof(GetPatient), new { id = result.Data }, result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PatientsDto>> GetPatient(Guid id)
        {
            try
            {
                var patient = await _mediator.Send(new GetPatientByUserIdQuery { Id = id });
                if (patient == null)
                {
                    return NotFound();
                }
                return Ok(patient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientsDto>>> GetAllPatients()
        {
            try
            {
                var patients = await _mediator.Send(new GetAllPatientsQuery());
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] UpdatePatientCommand command)
        {
            if (id != command.PatientId)
            {
                return BadRequest("Patient ID in the path does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound("Patient not found.");
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            try
            {
                await _mediator.Send(new DeletePatientCommand { PatientId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound($"Patient with ID {id} not found.");
            }
        }
    }
}
