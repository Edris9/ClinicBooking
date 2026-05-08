using System;

public class GetPatientByIdQuery : IRequest<PatientDto>
{
    public int Id { get; set; }
}