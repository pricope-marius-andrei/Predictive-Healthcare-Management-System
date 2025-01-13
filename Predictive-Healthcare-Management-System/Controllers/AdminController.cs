using Application.DTOs;
using Application.UseCases.Authentication;
using Application.UseCases.Commands.Admin;
using Application.UseCases.Commands.Doctor;
using Application.UseCases.Commands.Patient;
using Application.UseCases.Queries.Doctor;
using Application.UseCases.Queries.Patient;
using Application.Utils;
using Domain.Common;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Predictive_Healthcare_Management_System.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        private const string DoctorNotFound = "Doctor not found!";
        private const string PatientNotFound = "Patient not found!";

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("users")]
        [ProducesResponseType(typeof(Guid), 201)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserCommand command)
        {
            try
            {
                if (command.UserType != EUserType.Doctor && command.UserType != EUserType.Patient)
                {
                    return BadRequest("Invalid UserType specified. Only Doctor and Patient can be created via this endpoint.");
                }

                var userId = await _mediator.Send(command);

                if (userId == Guid.Empty)
                {
                    return BadRequest("A user with the same email already exists.");
                }

                return CreatedAtAction(nameof(GetUserById), new { id = userId }, new { UserId = userId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("users/{id:guid}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var doctorResult = await _mediator.Send(new GetDoctorByIdQuery { Id = id });
                if (doctorResult.IsSuccess && doctorResult.Data != null)
                {
                    return Ok(new { UserType = EUserType.Doctor.ToString(), Data = doctorResult.Data });
                }

                var patientResult = await _mediator.Send(new GetPatientByIdQuery { Id = id });
                if (patientResult.IsSuccess && patientResult.Data != null)
                {
                    return Ok(new { UserType = EUserType.Patient.ToString(), Data = patientResult.Data });
                }

                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("users")]
        [ProducesResponseType(typeof(IEnumerable<object>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetUsersByType([FromQuery] EUserType? userType = null)
        {
            try
            {
                IEnumerable<object> users = new List<object>();

                if (userType.HasValue)
                {
                    switch (userType.Value)
                    {
                        case EUserType.Doctor:
                            {
                                var doctorsResult = await _mediator.Send(new GetAllDoctorsQuery());
                                if (doctorsResult.IsSuccess && doctorsResult.Data != null)
                                {
                                    users = doctorsResult.Data.Select(d => new
                                    {
                                        UserType = EUserType.Doctor.ToString(),
                                        Data = (object)d
                                    });
                                }
                                else
                                {
                                    return BadRequest(doctorsResult.ErrorMessage ?? "Failed to retrieve doctors.");
                                }
                                break;
                            }
                        case EUserType.Patient:
                            {
                                var patientsResult = await _mediator.Send(new GetAllPatientsQuery());
                                if (patientsResult.IsSuccess && patientsResult.Data != null)
                                {
                                    users = patientsResult.Data.Select(p => new
                                    {
                                        UserType = EUserType.Patient.ToString(),
                                        Data = (object)p
                                    });
                                }
                                else
                                {
                                    return BadRequest(patientsResult.ErrorMessage ?? "Failed to retrieve patients.");
                                }
                                break;
                            }
                        default:
                            return BadRequest("Invalid UserType specified.");
                    }
                }
                else
                {
                    var doctorsResult = await _mediator.Send(new GetAllDoctorsQuery());
                    var patientsResult = await _mediator.Send(new GetAllPatientsQuery());

                    if (!doctorsResult.IsSuccess || !patientsResult.IsSuccess)
                    {
                        return BadRequest("Failed to retrieve some user types.");
                    }

                    var doctors = doctorsResult.Data ?? new List<DoctorDto>();
                    var patients = patientsResult.Data ?? new List<PatientDto>();

                    users = doctors.Select(d => new
                    {
                        UserType = EUserType.Doctor.ToString(),
                        Data = (object)d
                    })
                        .Concat(patients.Select(p => new
                        {
                            UserType = EUserType.Patient.ToString(),
                            Data = (object)p
                        }));
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("users/{id:guid}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand updateCommand)
        {
            if (id != updateCommand.Id)
            {
                return BadRequest("User ID in the path does not match the ID in the request body.");
            }

            try
            {
                var userType = updateCommand.UserType;

                switch (userType)
                {
                    case EUserType.Doctor:
                        var updateDoctorResult = await _mediator.Send(new UpdateDoctorCommand
                        {
                            Id = id,
                            Email = updateCommand.Email,
                            Username = updateCommand.Username,
                            FirstName = updateCommand.FirstName,
                            LastName = updateCommand.LastName,
                            PhoneNumber = updateCommand.PhoneNumber,
                            Specialization = updateCommand.Specialization,
                        });

                        if (updateDoctorResult.IsSuccess)
                        {
                            return Ok(new
                            {
                                UserType = EUserType.Doctor.ToString(),
                                Data = updateDoctorResult.Data
                            });
                        }
                        return BadRequest(updateDoctorResult.ErrorMessage ?? "Failed to update doctor.");

                    case EUserType.Patient:
                        var updatePatientResult = await _mediator.Send(new UpdatePatientCommand
                        {
                            Id = id,
                            Email = updateCommand.Email,
                            Username = updateCommand.Username,
                            FirstName = updateCommand.FirstName,
                            LastName = updateCommand.LastName,
                            PhoneNumber = updateCommand.PhoneNumber,
                            DateOfBirth = updateCommand.DateOfBirth,
                            Gender = updateCommand.Gender,
                            Height = updateCommand.Height,
                            Weight = updateCommand.Weight,
                            DoctorId = updateCommand.DoctorId
                        });

                        if (updatePatientResult.IsSuccess)
                        {
                            return Ok(new
                            {
                                UserType = EUserType.Patient.ToString(),
                                Data = updatePatientResult.Data
                            });
                        }
                        return BadRequest(updatePatientResult.ErrorMessage ?? "Failed to update patient.");

                    default:
                        return BadRequest("Invalid UserType specified.");
                }
            }
            catch (ValidationException vex)
            {
                return BadRequest(vex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("users/{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteDoctorCommand { Id = id });
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                try
                {
                    await _mediator.Send(new DeletePatientCommand { Id = id });
                    return NoContent();
                }
                catch (KeyNotFoundException)
                {
                    return NotFound("User not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("doctors/{doctorId:guid}/assign-patient")]
        [ProducesResponseType(typeof(DoctorDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> AssignPatientToDoctor(Guid doctorId, [FromBody] AssignPatientToDoctorRequest request)
        {
            if (doctorId != request.DoctorId)
            {
                return BadRequest("DoctorId in URL does not match DoctorId in request body.");
            }

            var command = new AssignPatientToDoctorCommand
            {
                DoctorId = request.DoctorId,
                PatientId = request.PatientId
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            if (result.ErrorMessage == DoctorNotFound || result.ErrorMessage == PatientNotFound)
            {
                return NotFound(result.ErrorMessage);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("doctors/{doctorId:guid}/remove-patient")]
        [ProducesResponseType(typeof(DoctorDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> RemovePatientFromDoctor(Guid doctorId, [FromBody] RemovePatientFromDoctorRequest request)
        {
            if (doctorId != request.DoctorId)
            {
                return BadRequest("DoctorId in URL does not match DoctorId in request body.");
            }

            var command = new RemovePatientFromDoctorCommand
            {
                DoctorId = request.DoctorId,
                PatientId = request.PatientId
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("users/sorted")]
        [ProducesResponseType(typeof(List<object>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetSortedUsers([FromQuery] EUserType userType, [FromQuery] string sortBy)
        {
            try
            {
                switch (userType)
                {
                    case EUserType.Doctor:
                        if (!Enum.TryParse(sortBy, true, out DoctorSortBy doctorSortBy))
                        {
                            return BadRequest("Invalid sortBy value for doctors.");
                        }

                        var sortedDoctors = await _mediator.Send(new GetDoctorsSortedQuery(doctorSortBy));
                        if (sortedDoctors.IsSuccess)
                        {
                            return Ok(sortedDoctors.Data);
                        }
                        return BadRequest(sortedDoctors.ErrorMessage);

                    case EUserType.Patient:
                        if (!Enum.TryParse(sortBy, true, out PatientSortBy patientSortBy))
                        {
                            return BadRequest("Invalid sortBy value for patients.");
                        }

                        var sortedPatients = await _mediator.Send(new GetPatientsSortedQuery(patientSortBy));
                        if (sortedPatients.IsSuccess)
                        {
                            return Ok(sortedPatients.Data);
                        }
                        return BadRequest(sortedPatients.ErrorMessage);

                    default:
                        return BadRequest("Invalid UserType specified.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("users/paginated")]
        [ProducesResponseType(typeof(Result<PagedResult<object>>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetPaginatedUsers(
            [FromQuery] EUserType userType,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? username = null)
        {
            try
            {
                switch (userType)
                {
                    case EUserType.Doctor:
                        var paginatedDoctors = await _mediator.Send(new GetPaginatedDoctorsQuery
                        {
                            Page = page,
                            PageSize = pageSize,
                            Username = username
                        });
                        if (paginatedDoctors.IsSuccess)
                            return Ok(paginatedDoctors.Data);
                        return BadRequest(paginatedDoctors.ErrorMessage);

                    case EUserType.Patient:
                        var paginatedPatients = await _mediator.Send(new GetPaginatedPatientsQuery
                        {
                            Page = page,
                            PageSize = pageSize,
                            Username = username
                        });
                        if (paginatedPatients.IsSuccess)
                            return Ok(paginatedPatients.Data);
                        return BadRequest(paginatedPatients.ErrorMessage);

                    default:
                        return BadRequest("Invalid UserType specified.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}