using MediatR;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Query
{
    public record GetPatientProfileQuery(string patientId,CancellationToken CancellationToken):IRequest<ResultResponse<PatientPeofileDTO>>;
    
}
