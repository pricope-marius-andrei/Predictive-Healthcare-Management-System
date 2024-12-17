using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.AIML
{
    public class SymptomDiseasePredictionModel
    {
        private readonly MLContext mlContext;
        private ITransformer model;
        private IDataView _trainingDataView;
        private string _modelFilePath;
        private string _trainigFilePath;

        public SymptomDiseasePredictionModel()
        {
            _trainigFilePath = "C:\\Users\\Pricope\\Documents\\Predictive-Healthcare-Management-System\\Predictive-Healthcare-Management-System\\dataset.csv";
            _modelFilePath = "C:\\Users\\Pricope\\Documents\\Predictive-Healthcare-Management-System\\Predictive-Healthcare-Management-System\\MLModule.zip";
            mlContext = new MLContext();
        }

        private IEstimator<ITransformer> ProcessData()
        {
            var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", "Disease")
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom1", outputColumnName: "Symptom1Subject"))
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom2", outputColumnName: "Symptom2Subject"))
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom3", outputColumnName: "Symptom3Subject"))
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom4", outputColumnName: "Symptom4Subject"))
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom5", outputColumnName: "Symptom5Subject"))
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom6", outputColumnName: "Symptom6Subject"))
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom7", outputColumnName: "Symptom7Subject"))
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom8", outputColumnName: "Symptom8Subject"))
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom9", outputColumnName: "Symptom9Subject"))
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom10", outputColumnName: "Symptom10Subject"))
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom11", outputColumnName: "Symptom11Subject"))
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom12", outputColumnName: "Symptom12Subject"))
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom13", outputColumnName: "Symptom13Subject"))
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom14", outputColumnName: "Symptom14Subject"))
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom15", outputColumnName: "Symptom15Subject"))
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom16", outputColumnName: "Symptom16Subject"))
            .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Symptom17", outputColumnName: "Symptom17Subject"))
            .Append(mlContext.Transforms.Concatenate("Features", "Symptom1Subject", "Symptom2Subject", "Symptom3Subject", "Symptom4Subject", "Symptom5Subject", "Symptom6Subject", "Symptom7Subject", "Symptom8Subject", "Symptom9Subject", "Symptom10Subject", "Symptom11Subject", "Symptom12Subject", "Symptom13Subject", "Symptom14Subject", "Symptom15Subject", "Symptom16Subject", "Symptom17Subject"))
            .AppendCacheCheckpoint(mlContext);

            return pipeline;
        }

        private IEstimator<ITransformer> BuildAndTrainModel(IDataView trainingDataView, IEstimator<ITransformer> pipeline)
        {
            var trainingPipeline = pipeline.Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            model = trainingPipeline.Fit(trainingDataView);

            return trainingPipeline;
        }


        private void SaveModelAsFile()
        {
            mlContext.Model.Save(model, _trainingDataView.Schema, _modelFilePath);
        }

        public void Train()
        {
            // Load data
            _trainingDataView = mlContext.Data.LoadFromTextFile<SymptomData>(_trainigFilePath, hasHeader: true, separatorChar: ',');

            // Define pipeline with text featurization
            var pipeline = ProcessData();

            Console.Write("Training...");

            var trainingPipeline = BuildAndTrainModel(_trainingDataView, pipeline);

            SaveModelAsFile();
        }

        private string PredictDiseaseForSubjectLine(SymptomData predictSymptomData)
        {
            PredictionEngine<SymptomData, SymptomDataPrediction> predictionEngine;
            var model = mlContext.Model.Load(_modelFilePath, out var modelInputSchema);

            Console.Write("Predicting...");
            predictionEngine = mlContext.Model.CreatePredictionEngine<SymptomData, SymptomDataPrediction>(model);
            var result = predictionEngine.Predict(predictSymptomData);

            Console.Write("Predicting...");
            return result.Disease;
        }

        public string Predict(SymptomData symptomData)
        {
            // Predict
            return PredictDiseaseForSubjectLine(symptomData);
        }
    }
}
