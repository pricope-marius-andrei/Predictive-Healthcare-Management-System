using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.Patient
{
    public class GetPatientByIdQuery : IRequest<Result<PatientDto>>
    {
        public Guid Id { get; set; }
    }
}