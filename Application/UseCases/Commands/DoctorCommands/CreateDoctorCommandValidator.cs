﻿using Application.UseCases.Commands.BaseCommands;
using Domain.Common;
using FluentValidation;

namespace Application.UseCases.Commands.DoctorCommands
{
	public class CreateDoctorCommandValidator : BaseUserCommandValidator<CreateDoctorCommand, Result<Guid>>
	{
		public CreateDoctorCommandValidator() : base()
		{

		}
	}
}