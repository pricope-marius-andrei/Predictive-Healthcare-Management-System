using Application.Use_Cases.Commands;
using MediatR;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Application.Use_Cases.Queries

namespace Predictive_Healthcare_Management_System.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]

    public class PatientsController : Controller
    {
        private readonly IMediator mediator;
        public PatientsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreatePatient(CreatePatientCommand command)
        {
            var id= await mediator.Send(command);
            return CreatedAtAction(nameof(GetPatient), new { id = id }, id);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PatientsDto>> GetPatient(Guid id)
        {
            var patient = await mediator.Send(new GetPatientByUserIdQuery { Id = id });
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }


    }
}
