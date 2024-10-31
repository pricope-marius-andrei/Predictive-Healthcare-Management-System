using Application.Use_Cases.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Predictive_Healthcare_Management_System.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]

    public class TestsController : ControllerBase
    {
        private readonly IMediator mediator;

        public TestsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateTest(CreateTestCommand command)
        {
            return await mediator.Send(command);
            
        }
    }
}
