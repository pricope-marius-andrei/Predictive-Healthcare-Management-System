using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.Doctor
{
    public class GetAllDoctorsQuery : IRequest<Result<IEnumerable<DoctorDto>>>
    {
    }
}
