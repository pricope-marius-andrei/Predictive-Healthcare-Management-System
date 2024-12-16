using Application.UseCases.CommandHandlers.MedicalHistory;
using Application.UseCases.Commands.MedicalHistory;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;
using Xunit;

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
    public class DeleteMedicalHistoryCommandHandlerTests
    {
        private readonly IMedicalHistoryRepository _mockMedicalHistoryRepository;
        private readonly DeleteMedicalHistoryCommandHandler _handler;

        public DeleteMedicalHistoryCommandHandlerTests()
        {
            _mockMedicalHistoryRepository = Substitute.For<IMedicalHistoryRepository>();
            _handler = new DeleteMedicalHistoryCommandHandler(_mockMedicalHistoryRepository);
        }
    }
}





