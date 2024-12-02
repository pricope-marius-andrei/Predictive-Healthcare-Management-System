using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.Doctor
{
    public class GetAllDoctorsQuery : IRequest<IEnumerable<DoctorDto>>
    {
    }
}
