﻿using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Commands
{
    public class UpdateDoctorCommand : IRequest<Result<Doctor>>
    {
        public required Guid DoctorId { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Specialization { get; set; }
        public required string PhoneNumber { get; set; }
    }
}