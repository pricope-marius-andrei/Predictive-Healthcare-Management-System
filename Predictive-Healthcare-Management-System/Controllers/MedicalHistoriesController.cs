using MediatR;
using Application.DTOs;
using Application.UseCases.Commands;
using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Queries;
using Domain.Entities;
using Domain.Common;

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

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateMedicalHistory(CreateMedicalHistoryCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetMedicalHistoryById), new { id = result.Data }, result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MedicalHistoryDto>> GetMedicalHistoryById(Guid id)
        {
            try
            {
                var medicalHistory = await _mediator.Send(new GetMedicalHistoryByIdQuery { HistoryId = id });
                return Ok(medicalHistory);
            }
            catch (Exception ex)
            {
                return NotFound($"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalHistoryDto>>> GetAllMedicalHistories()
        {
            try
            {
                var medicalHistories = await _mediator.Send(new GetAllMedicalHistoriesQuery());
                return Ok(medicalHistories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Result<MedicalHistory>>> UpdateMedicalHistory(Guid id, UpdateMedicalHistoryCommand command)
        {
            if (id != command.HistoryId)
            {
                return BadRequest("Medical history ID in the path does not match the ID in the request body.");
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
        public async Task<ActionResult> DeleteMedicalHistory(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteMedicalHistoryCommand { HistoryId = id });
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return NotFound($"Medical history with ID {id} not found.");
            }
        }
    }
}
