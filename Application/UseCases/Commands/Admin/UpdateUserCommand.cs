using Application.UseCases.Authentication;

namespace Application.UseCases.Commands.Admin;

public class UpdateUserCommand
{
    public Guid Id { get; set; }
    public EUserType UserType { get; set; }
    // -----------------------------------
    // Common Properties (Applicable to all user types)
    // -----------------------------------
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
    // -----------------------------------
    // Doctor-Specific Properties
    // -----------------------------------
    public string? Specialization { get; set; }
    public List<Guid>? PatientIds { get; set; }
    // -----------------------------------
    // Patient-Specific Properties
    // -----------------------------------
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public Guid? DoctorId { get; set; }
}