using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetAllDoctorsQuery : IRequest<IEnumerable<DoctorDto>>
    {
    }
}
