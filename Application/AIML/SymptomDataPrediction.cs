using Microsoft.ML.Data;

namespace Application.AIML
{
    public class SymptomDataPrediction
    {
        [ColumnName("PredictedLabel")]
        public string Disease;
    }
}
