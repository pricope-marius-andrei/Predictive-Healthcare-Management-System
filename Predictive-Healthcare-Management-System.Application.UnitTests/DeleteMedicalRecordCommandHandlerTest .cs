using Application.UseCases.CommandHandlers.MedicalRecord;
using Application.UseCases.Commands.MedicalRecord;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;
using Xunit;

namespace Predictive_Healthcare_Management_System.Application.UnitTests
{
    public class DeleteMedicalRecordCommandHandlerTests
    {
        private readonly IMedicalRecordRepository _mockMedicalRecordRepository;
        private readonly DeleteMedicalRecordCommandHandler _handler;

        public DeleteMedicalRecordCommandHandlerTests()
        {
            _mockMedicalRecordRepository = Substitute.For<IMedicalRecordRepository>();
            _handler = new DeleteMedicalRecordCommandHandler(_mockMedicalRecordRepository);
        }
    }
}






