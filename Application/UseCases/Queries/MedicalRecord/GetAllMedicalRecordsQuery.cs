using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.MedicalRecord
{
    public class GetAllMedicalRecordsQuery : IRequest<IEnumerable<MedicalRecordDto>>
    {
    }
}