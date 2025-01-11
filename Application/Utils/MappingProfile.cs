using Application.DTOs;
using Application.UseCases.Commands.Doctor;
using Application.UseCases.Commands.MedicalHistory;
using Application.UseCases.Commands.MedicalRecord;
using Application.UseCases.Commands.Patient;
using AutoMapper;
using Domain.Common;
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
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
                .ForMember(dest => dest.MedicalHistories, opt => opt.MapFrom(src => src.MedicalHistories))
                .ForMember(dest => dest.MedicalRecords, opt => opt.MapFrom(src => src.MedicalRecords));

            // CreateMap<CreatePatientCommand, Patient>();
            CreateMap<UpdatePatientCommand, Patient>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

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
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Patients, opt => opt.MapFrom(src => src.Patients));

            CreateMap<UpdateDoctorCommand, Doctor>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<AssignPatientToDoctorRequest, AssignPatientToDoctorCommand>();
            CreateMap<RemovePatientFromDoctorRequest, RemovePatientFromDoctorCommand>();
        }
    }
}