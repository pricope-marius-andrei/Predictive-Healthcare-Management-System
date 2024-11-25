using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Domain.Common;
using Domain.Entities;
using Application.UseCases.Commands.MedicalRecordCommands;
using Application.UseCases.Queries.MedicalRecordsQueries;

namespace Predictive_Healthcare_Management_System.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MedicalRecordsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> GetAllMedicalRecords()
        {
            try
            {
                var medicalRecords = await mediator.Send(new GetAllMedicalRecordsQuery());
                return Ok(medicalRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MedicalRecordDto>> GetMedicalRecordById(Guid id)
        {
            try
            {
                var medicalRecord = await mediator.Send(new GetMedicalRecordByIdQuery { RecordId = id });
                return Ok(medicalRecord);
            }
            catch (Exception ex)
            {
                return NotFound($"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("patient/{PersonId}")]
        public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> GetMedicalRecordsByPersonId(Guid PersonId)
        {
            try
            {
                var query = new GetMedicalRecordsByPatientIdQuery { PatientId = PersonId };
                var result = await mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateMedicalRecord(CreateMedicalRecordCommand command)
        {
            try
            {
                var result = await mediator.Send(command);
                if (result.IsSuccess)
                {
                    return CreatedAtAction(nameof(GetMedicalRecordById), new { id = result.Data }, result.Data);
                }
                return BadRequest(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Result<MedicalRecord>>> UpdateMedicalRecord(Guid id, UpdateMedicalRecordCommand command)
        {
            if (id != command.RecordId)
            {
                return BadRequest("Medical record ID in the path does not match the ID in the request body.");
            }

            try
            {
                var result = await mediator.Send(command);
                if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }
                return BadRequest(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteMedicalRecord(Guid id)
        {
            try
            {
                await mediator.Send(new DeleteMedicalRecordCommand { RecordId = id });
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch
            {
                return NotFound($"Medical record with ID {id} not found.");
            }
        }
    }
}