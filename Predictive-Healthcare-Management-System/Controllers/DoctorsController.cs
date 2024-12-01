using Application.DTOs;
using Application.UseCases.Commands.Doctor;
using Application.UseCases.Queries;
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

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateDoctor(CreateDoctorCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetDoctorById), new { id = result.Data }, result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
                await _mediator.Send(new DeleteDoctorCommand { DoctorId = id });
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
            if (id != command.DoctorId)
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
    }
}   
