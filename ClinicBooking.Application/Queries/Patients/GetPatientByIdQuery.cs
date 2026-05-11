using System;
using MediatR;

public class GetPatientByIdQuery : IRequest<PatientDto>
{
    public int Id { get; set; }
}