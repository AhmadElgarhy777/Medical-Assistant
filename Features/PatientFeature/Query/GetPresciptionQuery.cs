using Azure;
using MediatR;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Query
{
    public record GetPresciptionQuery(string Id,int page=1):IRequest<List<PresciptionDTO>>;
   
}
