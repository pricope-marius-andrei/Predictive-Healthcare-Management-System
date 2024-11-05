using Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Application.Use_Cases.Queries
{
    public class GetAllPatientsQuery : IRequest<IEnumerable<PatientsDto>>
    {
    }
}