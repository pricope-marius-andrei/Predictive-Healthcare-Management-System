using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<Guid>> CreatePatient()
        {
            return NoContent();
            //return await mediator.Send(command);

        }
    }
}
