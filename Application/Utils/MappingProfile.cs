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
            // -----------------------------
            // Patient Mappings
            // -----------------------------
            CreateMap<Patient, PatientDto>()
                .ForMember(dest => dest.MedicalHistories, opt => opt.MapFrom(src => src.MedicalHistories))
                .ForMember(dest => dest.MedicalRecords, opt => opt.MapFrom(src => src.MedicalRecords));

            CreateMap<CreatePatientCommand, Patient>();
            CreateMap<UpdatePatientCommand, Patient>();

            // -----------------------------
            // Medical History Mappings
            // -----------------------------
            CreateMap<MedicalHistory, MedicalHistoryDto>();
            CreateMap<CreateMedicalHistoryCommand, MedicalHistory>();
            CreateMap<UpdateMedicalHistoryCommand, MedicalHistory>();

            // -----------------------------
            // Medical Record Mappings
            // -----------------------------
            CreateMap<MedicalRecord, MedicalRecordDto>();
            CreateMap<CreateMedicalRecordCommand, MedicalRecord>();
            CreateMap<UpdateMedicalRecordCommand, MedicalRecord>();

            // -----------------------------
            // Doctor Mappings
            // -----------------------------
            CreateMap<Doctor, DoctorDto>();

            CreateMap<CreateDoctorCommand, Doctor>();
            CreateMap<UpdateDoctorCommand, Doctor>();
        }
    }
}