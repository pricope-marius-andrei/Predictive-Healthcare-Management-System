using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.Patient
{
    public class GetPatientsByUsernameFilterQuery : IRequest<IEnumerable<PatientDto>>
    {
        public string Username { get; }

        public GetPatientsByUsernameFilterQuery(string username)
        {
            Username = username;
        }
    }
}