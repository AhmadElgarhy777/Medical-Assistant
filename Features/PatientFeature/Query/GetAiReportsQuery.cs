using MediatR;
using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Query
{
    public record GetAiReportsQuery(string Id ,int page=1):IRequest<ResultResponse<List<AiReportDTO>>>;
    
}
