using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.Doctor
{
    public class GetDoctorByIdQuery : IRequest<Result<DoctorDto>>
    {
        public Guid Id { get; set; }
    }
}
