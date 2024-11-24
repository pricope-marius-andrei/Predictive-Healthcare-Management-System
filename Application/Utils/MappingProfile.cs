using Application.DTOs;
using Application.UseCases.Commands.DoctorCommands;
using Application.UseCases.Commands.MedicalHistoryCommands;
using Application.UseCases.Commands.MedicalRecordCommands;
using Application.UseCases.Commands.PatientCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
			CreateMap<Patient, PatientDto>()
				.ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PersonId))
				.ReverseMap()
				.ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PatientId));
			CreateMap<CreatePatientCommand, Patient>().ReverseMap();
            CreateMap<UpdatePatientCommand, Patient>().ReverseMap();

            CreateMap<MedicalHistory, MedicalHistoryDto>().ReverseMap();
            CreateMap<CreateMedicalHistoryCommand, MedicalHistory>().ReverseMap();
            CreateMap<UpdateMedicalHistoryCommand, MedicalHistory>().ReverseMap();

            CreateMap<MedicalRecord, MedicalRecordDto>().ReverseMap();
            CreateMap<CreateMedicalRecordCommand, MedicalRecord>().ReverseMap();
            CreateMap<UpdateMedicalRecordCommand, MedicalRecord>().ReverseMap();

			CreateMap<Doctor, DoctorDto>()
				.ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.PersonId))
				.ReverseMap()
				.ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.DoctorId));
			CreateMap<CreateDoctorCommand, Doctor>().ReverseMap();
            CreateMap<UpdateDoctorCommand, Doctor>().ReverseMap();
        }
    }
}