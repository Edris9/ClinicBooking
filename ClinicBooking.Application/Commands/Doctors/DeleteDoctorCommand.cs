using MediatR;
using AutoMapper;

public class DeleteDoctorCommand : IRequest
{
    public int Id { get; set; }
}