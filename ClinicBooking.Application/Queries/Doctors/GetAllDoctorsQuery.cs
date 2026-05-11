using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class GetAllDoctorsQuery : IRequest<List<DoctorDto>>
{
    public GetAllDoctorsQuery()
    {
    }
}