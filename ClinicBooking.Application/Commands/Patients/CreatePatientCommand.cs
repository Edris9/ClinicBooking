using MediatR;
using AutoMapper;       

using System;

public class CreatePatientCommand: IRequest<int>
{
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

}