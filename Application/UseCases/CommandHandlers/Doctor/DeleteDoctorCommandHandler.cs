using Application.UseCases.Commands.Doctor;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.Doctor
{
    public class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand>
    {
        private readonly IDoctorRepository _repository;

        public DeleteDoctorCommandHandler(IDoctorRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = await _repository.GetByIdAsync(request.DoctorId);
            if (doctor == null)
            {
                throw new InvalidOperationException("Doctor not found.");
            }

            await _repository.DeleteAsync(request.DoctorId);
        }
    }
}
