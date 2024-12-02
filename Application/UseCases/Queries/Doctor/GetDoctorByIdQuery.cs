using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.Doctor
{
    public class GetDoctorByIdQuery : IRequest<DoctorDto>
    {
        public Guid Id { get; set; }
    }
}
