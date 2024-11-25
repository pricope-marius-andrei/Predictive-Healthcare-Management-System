using Application.DTOs;
using Application.UseCases.Commands.DoctorCommands;
using Application.UseCases.Queries.DoctorQueries;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Predictive_Healthcare_Management_System.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DoctorsController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateDoctor(CreateDoctorCommand command)
        {
            try
            {
                var result = await mediator.Send(command);
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
                var doctor = await mediator.Send(new GetDoctorByIdQuery { DoctorId = id });
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
                await mediator.Send(new DeleteDoctorCommand { PersonId = id });
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
                var doctors = await mediator.Send(new GetAllDoctorsQuery());
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
                var result = await mediator.Send(command);

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}