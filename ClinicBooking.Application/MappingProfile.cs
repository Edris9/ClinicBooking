
using System;
using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Doctor, DoctorDto>();
        CreateMap<Patient, PatientDto>();
        CreateMap<Appointment, AppointmentDto>();
    }
}