using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.Doctor
{
    public class GetDoctorsByUsernameFilterQuery : IRequest<IEnumerable<DoctorDto>>
    {
        public string Username { get; }

        public GetDoctorsByUsernameFilterQuery(string username)
        {
            Username = username;
        }
    }
}