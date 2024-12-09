using Application.DTOs;
using Application.Utils;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.Doctor
{
    public class GetPatientsByDoctorIdQuery : IRequest<Result<PagedResult<PatientDto>>>
    {
        public Guid DoctorId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Username { get; set; }

        public GetPatientsByDoctorIdQuery(Guid doctorId)
        {
            DoctorId = doctorId;
        }
    }
}