using Application.DTOs;
using Application.UseCases.Commands.Doctor;
using Application.UseCases.Commands.MedicalHistory;
using Application.UseCases.Commands.MedicalRecord;
using Application.UseCases.Commands.Patient;
using AutoMapper;
using Domain.Entities;

namespace Application.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<CreatePatientCommand, Patient>().ReverseMap();
            CreateMap<UpdatePatientCommand, Patient>().ReverseMap();
            CreateMap<MedicalHistory, MedicalHistoryDto>().ReverseMap();
            CreateMap<CreateMedicalHistoryCommand, MedicalHistory>().ReverseMap();
            CreateMap<UpdateMedicalHistoryCommand, MedicalHistory>().ReverseMap();
            CreateMap<MedicalRecord, MedicalRecordDto>().ReverseMap();
            CreateMap<CreateMedicalRecordCommand, MedicalRecord>().ReverseMap();
            CreateMap<UpdateMedicalRecordCommand, MedicalRecord>().ReverseMap();
            CreateMap<Doctor, DoctorDto>().ReverseMap();
            CreateMap<CreateDoctorCommand, Doctor>().ReverseMap();
            CreateMap<UpdateDoctorCommand, Doctor>().ReverseMap();
        }
    }
}
