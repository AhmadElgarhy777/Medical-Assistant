using Azure.Core;
using MediatR;
using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Query
{
    public record GetAllDoctorsSearchQuery(SearchingFieldsDTO searching,int page = 1):IRequest<List<DoctorDTO>>;
    
}
