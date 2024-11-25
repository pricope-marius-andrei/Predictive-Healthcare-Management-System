using Application.UseCases.Commands.DoctorCommands;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.DoctorCommandHandlers
{
    public class DeleteDoctorCommandHandler(IDoctorRepository repository) : IRequestHandler<DeleteDoctorCommand>
    {
        public async Task Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctor = await repository.GetByIdAsync(request.PersonId);
            if (doctor == null)
            {
                throw new InvalidOperationException("Doctor not found.");
            }

            await repository.DeleteAsync(request.PersonId);
        }
    }
}