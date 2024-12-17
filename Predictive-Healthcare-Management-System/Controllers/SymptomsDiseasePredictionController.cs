using Application.AIML;
using Microsoft.AspNetCore.Mvc;

namespace Predictive_Healthcare_Management_System.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SymptomsDiseasePredictionController : ControllerBase
    {
        private readonly SymptomDiseasePredictionModel syntomDiseasePredictionModel;
        public SymptomsDiseasePredictionController()
        {
            syntomDiseasePredictionModel = new SymptomDiseasePredictionModel();
            syntomDiseasePredictionModel.Train();
        }

        [HttpPost("predict")]
        public ActionResult Predict([FromBody] SymptomData symptomData)
        {
            var prediction = syntomDiseasePredictionModel.Predict(symptomData);
            return Ok(new { predictedDisease = prediction });
        }
    }
}
