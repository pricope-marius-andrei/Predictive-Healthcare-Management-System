using Application.DTOs;
using Application.UseCases.Commands.Doctor;
using Application.UseCases.Queries.Doctor;
using Application.Utils;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Predictive_Healthcare_Management_System.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DoctorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DoctorDto>> GetDoctorById(Guid id)
        {
            try
            {
                var doctor = await _mediator.Send(new GetDoctorByIdQuery { Id = id });
                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return NotFound($"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteDoctor(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteDoctorCommand { Id = id });
                return NoContent();
            }
            catch
            {
                return NotFound($"Doctor with ID {id} Not Found.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetAllDoctors()
        {
            try
            {
                var doctors = await _mediator.Send(new GetAllDoctorsQuery());
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Result<Doctor>>> UpdateDoctor(Guid id, UpdateDoctorCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest($"Doctor {id} in the path does not match the ID in the request body.");
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

        [HttpGet("paginated")]
        public async Task<ActionResult<PagedResult<DoctorDto>>> GetPaginatedDoctors([FromQuery] int page, [FromQuery] int pageSize)
        {
            var query = new GetPaginatedDoctorsQuery
            {
                Page = page,
                PageSize = pageSize
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("sorted")]
        public async Task<ActionResult<Result<List<DoctorDto>>>> GetSortedDoctors([FromQuery] DoctorSortBy sortBy)
        {
            var query = new GetDoctorsSortedQuery(sortBy);
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetDoctorsByUsername([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username query parameter is required.");
            }

            var query = new GetDoctorsByUsernameFilterQuery(username);
            var doctors = await _mediator.Send(query);
            return Ok(doctors);
        }

        [HttpPost("{doctorId}/assign-patient")]
        [ProducesResponseType(typeof(Doctor), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> AssignPatientToDoctor(Guid doctorId, [FromBody] AssignPatientToDoctorRequest request)
        {
            if (doctorId != request.DoctorId)
            {
                return BadRequest("DoctorId in URL does not match DoctorId in request body.");
            }

            var command = new AssignPatientToDoctorCommand
            {
                DoctorId = request.DoctorId,
                PatientId = request.PatientId
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("{doctorId}/remove-patient")]
        [ProducesResponseType(typeof(Doctor), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> RemovePatientFromDoctor(Guid doctorId, [FromBody] RemovePatientFromDoctorRequest request)
        {
            if (doctorId != request.DoctorId)
            {
                return BadRequest("DoctorId in URL does not match DoctorId in request body.");
            }

            var command = new RemovePatientFromDoctorCommand
            {
                DoctorId = request.DoctorId,
                PatientId = request.PatientId
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("doctor/{doctorId}/paginated")]
        [ProducesResponseType(typeof(Result<PagedResult<PatientDto>>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetPaginatedPatientsByDoctor(
            [FromRoute] Guid doctorId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? username = null)
        {
            var query = new GetPatientsByDoctorIdQuery(doctorId)
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
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
    }
}   
