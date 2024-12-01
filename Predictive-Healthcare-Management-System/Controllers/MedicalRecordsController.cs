using Application.DTOs;
using Application.UseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Domain.Common;
using Domain.Entities;
using Application.Utils;
using Application.UseCases.Commands.MedicalRecord;
using Application.UseCases.Queries.MedicalRecord;
using Application.UseCases.Queries.MedicalHistory;
using Application.UseCases.Queries.Doctor;

namespace Predictive_Healthcare_Management_System.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicalRecordsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> GetAllMedicalRecords()
        {
            try
            {
                var medicalRecords = await _mediator.Send(new GetAllMedicalRecordsQuery());
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
                var medicalRecord = await _mediator.Send(new GetMedicalRecordByIdQuery { RecordId = id });
                return Ok(medicalRecord);
            }
            catch (Exception ex)
            {
                return NotFound($"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> GetMedicalRecordsByPatientId(Guid patientId)
        {
            try
            {
                var query = new GetMedicalRecordsByPatientIdQuery { PatientId = patientId };
                var result = await _mediator.Send(query);
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
                var result = await _mediator.Send(command);
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
                var result = await _mediator.Send(command);
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
                await _mediator.Send(new DeleteMedicalRecordCommand { RecordId = id });
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch
            {
                return NotFound($"Medical record with ID {id} not found.");
            }
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<PagedResult<MedicalRecordDto>>> GetPaginatedMedicalRecords([FromQuery] int page, [FromQuery] int pageSize)
        {
            var query = new GetPaginatedMedicalRecordsQuery
            {
                Page = page,
                PageSize = pageSize
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}