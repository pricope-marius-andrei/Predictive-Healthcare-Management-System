using Application.DTOs;
using Application.UseCases.Commands.MedicalHistory;
using Application.UseCases.Queries.MedicalHistory;
using Application.Utils;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Predictive_Healthcare_Management_System.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MedicalHistoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicalHistoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalHistoryDto>>> GetAllMedicalHistories()
        {
            var result = await _mediator.Send(new GetAllMedicalHistoriesQuery());
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MedicalHistoryDto>> GetMedicalHistoryById(Guid id)
        {
            var result = await _mediator.Send(new GetMedicalHistoryByIdQuery { HistoryId = id });
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> GetMedicalHistoriesByPatientId(Guid patientId)
        {
            var query = new GetMedicalHistoriesByPatientIdQuery { PatientId = patientId };
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateMedicalHistory(CreateMedicalHistoryCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetMedicalHistoryById), new { id = result.Data }, result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Result<MedicalHistory>>> UpdateMedicalHistory(Guid id, UpdateMedicalHistoryCommand command)
        {
            if (id != command.HistoryId)
            {
                return BadRequest("Medical history ID in the path does not match the ID in the request body.");
            }

            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(500, result.ErrorMessage);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteMedicalHistory(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteMedicalHistoryCommand { HistoryId = id });
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch
            {
                return NotFound($"Medical history with ID {id} not found.");
            }
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<PagedResult<MedicalHistoryDto>>> GetPaginatedMedicalHistories([FromQuery] int page, [FromQuery] int pageSize)
        {
            var query = new GetPaginatedMedicalHistoriesQuery
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
        public async Task<ActionResult<Result<List<MedicalHistoryDto>>>> GetSortedMedicalHistories([FromQuery] MedicalHistorySortBy sortBy)
        {
            var query = new GetMedicalHistoriesSortedQuery(sortBy);
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}

