using MediatR;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Common;
using Application.Utils;
using Application.UseCases.Commands.Patient;
using Application.UseCases.Queries.Patient;

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

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PatientDto>> GetPatientById(Guid id)
        {
            try
            {
                var patient = await _mediator.Send(new GetPatientByIdQuery { Id = id });
                return Ok(patient);
            }
            catch (Exception ex)
            {
                return NotFound($"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAllPatients()
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
        public async Task<ActionResult<Result<Patient>>> UpdatePatient(Guid id, UpdatePatientCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Patient ID in the path does not match the ID in the request body.");
            }

            try
            {
                var result = await _mediator.Send(command);

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            try
            {
                await _mediator.Send(new DeletePatientCommand { Id = id });
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch
            {
                return NotFound($"Patient with ID {id} not found.");
            }
        }

        [HttpGet("paginated")]
        [ProducesResponseType(typeof(Result<PagedResult<PatientDto>>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetPaginatedPatients(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? username = null)
        {
            var query = new GetPaginatedPatientsQuery
            {
                Page = page,
                PageSize = pageSize,
                Username = username
            };

            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("sorted")]
        public async Task<ActionResult<Result<List<PatientDto>>>> GetSortedPatients([FromQuery] PatientSortBy sortBy)
        {
            try
            {
                var query = new GetPatientsSortedQuery(sortBy);
                var result = await _mediator.Send(query);
                if (result.IsSuccess)
                    return Ok(result.Data);
                return BadRequest(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
