using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetAllMedicalRecordsQuery : IRequest<IEnumerable<MedicalRecordDto>>
    {
    }
}