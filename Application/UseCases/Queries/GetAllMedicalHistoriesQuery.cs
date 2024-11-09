﻿using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetAllMedicalHistoriesQuery : IRequest<IEnumerable<MedicalHistoryDto>>
    {
    }
}