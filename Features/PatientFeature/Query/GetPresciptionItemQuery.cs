using MediatR;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Query
{
    public record GetPresciptionItemQuery(string presciptionId, int page=1):IRequest<List<PresciptionItemDTO>>;
   
}
