using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.Patient
{
    public class GetAllPatientsQuery : IRequest<Result<IEnumerable<PatientDto>>>
    {
    }
}