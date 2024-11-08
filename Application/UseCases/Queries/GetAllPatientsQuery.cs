using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetAllPatientsQuery : IRequest<IEnumerable<PatientDto>>
    {
    }
}