using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Domain.Common;
using Domain.Entities;
using Application.Utils;
using Application.UseCases.Commands.MedicalRecord;
using Application.UseCases.Queries.MedicalRecord;

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
            var result = await _mediator.Send(new GetAllMedicalRecordsQuery());
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MedicalRecordDto>> GetMedicalRecordById(Guid id)
        {
            var result = await _mediator.Send(new GetMedicalRecordByIdQuery { RecordId = id });
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> GetMedicalRecordsByPatientId(Guid patientId)
        {
            var query = new GetMedicalRecordsByPatientIdQuery { PatientId = patientId };
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateMedicalRecord(CreateMedicalRecordCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetMedicalRecordById), new { id = result.Data }, result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Result<MedicalRecord>>> UpdateMedicalRecord(Guid id, UpdateMedicalRecordCommand command)
        {
            if (id != command.RecordId)
            {
                return BadRequest("Medical record ID in the path does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
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
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [HttpGet("sorted")]
        public async Task<ActionResult<Result<List<MedicalRecordDto>>>> GetSortedMedicalRecords([FromQuery] MedicalRecordSortBy sortBy)
        {
            var query = new GetMedicalRecordsSortedQuery(sortBy);
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}

