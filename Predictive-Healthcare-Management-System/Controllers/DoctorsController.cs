using Application.DTOs;
using Application.UseCases.Commands.Doctor;
using Application.UseCases.Commands.MedicalHistory;
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
            var result = await _mediator.Send(new GetDoctorByIdQuery { Id = id });
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteDoctor(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteDoctorCommand { Id = id });
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch
            {
                return NotFound($"Medical history with ID {id} not found.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetAllDoctors()
        {
            var result = await _mediator.Send(new GetAllDoctorsQuery());
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Result<Doctor>>> UpdateDoctor(Guid id, UpdateDoctorCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest($"Doctor {id} in the path does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [HttpGet("sorted")]
        public async Task<ActionResult<Result<List<DoctorDto>>>> GetSortedDoctors([FromQuery] DoctorSortBy sortBy)
        {
            var query = new GetDoctorsSortedQuery(sortBy);
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("paginated")]
        [ProducesResponseType(typeof(Result<PagedResult<DoctorDto>>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetPaginatedDoctors(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? username = null)
        {
            var query = new GetPaginatedDoctorsQuery
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

            return BadRequest(result.ErrorMessage);
        }
    }
}

