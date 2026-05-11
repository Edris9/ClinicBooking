using MediatR;
using AutoMapper;
using System;

public class DeletePatientCommand : IRequest
{
    public int Id { get; set; }
}