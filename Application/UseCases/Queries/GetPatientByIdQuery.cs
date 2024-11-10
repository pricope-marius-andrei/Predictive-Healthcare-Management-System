using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetPatientByIdQuery : IRequest<PatientDto>
    {
        public Guid Id { get; set; }
    }
}