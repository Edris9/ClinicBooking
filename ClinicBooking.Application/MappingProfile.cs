
using AutoMapper;
using System;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Doctor, DoctorDto>();
        CreateMap<Patient, PatientDto>();
        CreateMap<Appointment, AppointmentDto>();
        CreateMap<Payment, PaymentDto>();
    }
}