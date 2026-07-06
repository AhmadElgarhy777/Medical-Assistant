using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Query
{
    public record GetNurseDetailsByIdQuery(string NurseId) : IRequest<ResultResponse<SearchNurseDetailsResultByIdDto>>; 
   
}
