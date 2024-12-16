using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.Doctor
{
    public class GetDoctorsByUsernameFilterQuery : IRequest<Result<IEnumerable<DoctorDto>>>
    {
        public string Username { get; }

        public GetDoctorsByUsernameFilterQuery(string username)
        {
            Username = username;
        }
    }
}