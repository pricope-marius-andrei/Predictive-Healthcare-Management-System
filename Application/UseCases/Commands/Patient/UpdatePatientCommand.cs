﻿using Application.UseCases.Commands.Base;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Commands.Patient
{
    public class UpdatePatientCommand : BasePatientCommand<Result<Domain.Entities.Patient>>
    {
        public Guid PatientId { get; set; }
    }
}