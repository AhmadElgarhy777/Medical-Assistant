using MediatR;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Query
{
    public record GetDoctorsForPatientQuery(string PatientID):IRequest<ResultResponse<List<MyDoctorsDto>>>;
  
}
