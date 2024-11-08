using Application.DTOs;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetPatientByUserIdQuery : IRequest<PatientsDto>
    {
        public Guid Id { get; set; }
    }
}