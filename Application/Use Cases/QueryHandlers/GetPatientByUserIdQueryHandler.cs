using Application.DTOs;
using Application.Use_Cases.Queries;
using Domain.Entities;
using MediatR;

namespace Application.Use_Cases.QueryHandlers
{
    internal class GetPatientByUserIdQueryHandler : IRequestHandler<GetPatientByUserIdQuery, PatientsDto>
    {
        private readonly IPatientRepository repository;
        public GetPatientByUserIdQueryHandler(IPatientRepository repository)
        {
            this.repository = repository;
        }

        public async Task<PatientsDto> Handle(GetPatientByUserIdQuery request, CancellationToken cancellationToken)
        {
            var patient = await repository.GetByIdAsync(request.Id);
            return new PatientsDto
            {
                UserId = patient.UserId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                PhoneNumber = patient.PhoneNumber,
                Address = patient.Address
            };
        }
    }
}
