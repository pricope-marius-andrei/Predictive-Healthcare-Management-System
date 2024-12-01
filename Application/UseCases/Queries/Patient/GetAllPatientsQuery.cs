using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.Patient
{
    public class GetAllPatientsQuery : IRequest<IEnumerable<PatientDto>>
    {
    }
}