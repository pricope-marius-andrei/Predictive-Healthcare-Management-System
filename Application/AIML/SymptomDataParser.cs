using CsvHelper;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;

namespace Application.AIML
{
    public static class SymptomDataParser
    {
        public static List<SymptomData> LoadSymptomData(string filePath)
        {
            var symptomDataList = new List<SymptomData>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                   
                    var newSymptomData = new SymptomData();

                    for (int i = 1; i <= 17; i++) 
                    {
                        var symptom = csv.GetField<string>($"Symptom_{i}");
                        
                        if (!string.IsNullOrWhiteSpace(symptom)) 
                        {
                            typeof(SymptomData).GetProperty($"Symptom_{i}").SetValue(newSymptomData,symptom);
                        }
                    }


                    var disease = csv.GetField<string>("Disease");

                    newSymptomData.Disease = disease;
                    symptomDataList.Add(newSymptomData);
                }
            }
            return symptomDataList;
        }

    }
}
