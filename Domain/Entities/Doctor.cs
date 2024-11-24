namespace Domain.Entities
{
	public class Doctor : Person
	{
		public string Specialization { get; set; } = string.Empty;
		public ICollection<MedicalRecord>? MedicalRecords { get; set; }
	}
}