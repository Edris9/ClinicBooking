using MediatR;
using System;


public class GetDoctorByIdQuery : IRequest<DoctorDto>
{
    public int Id { get; set; }
   
}