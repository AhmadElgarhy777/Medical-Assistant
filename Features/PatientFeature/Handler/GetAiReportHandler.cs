using AutoMapper;
using DataAccess.EntittySpecifcation;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using Features.PatientFeature.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Handler
{
    public class GetAiReportHandler : IRequestHandler<GetAiReportsQuery, ResultResponse<List<AiReportDTO>>>
    {
        private readonly IPatientRepositry patientRepositry;
        private readonly IAiReportRepositry reportRepositry;
        private readonly IMapper mapper;

        public GetAiReportHandler(IPatientRepositry patientRepositry,IAiReportRepositry reportRepositry,IMapper mapper) 
        {
            this.patientRepositry = patientRepositry;
            this.reportRepositry = reportRepositry;
            this.mapper = mapper;
        }
        public async Task<ResultResponse<List<AiReportDTO>>> Handle(GetAiReportsQuery request, CancellationToken cancellationToken)
        {
            var patientId = request.Id;
            var page = request.page;
            if (page <= 0) page = 1;

            var spec = new AiReportsSpesfication(e=>e.PatientId==patientId);
            var Reports = reportRepositry.GetAll(spec);
            
            var pagnation = Reports.Skip((page - 1) * 5).Take(5);
           
            var ReportsList = await pagnation.ToListAsync(cancellationToken);
            if (ReportsList is not null)
            {
                    var ReportDTO= mapper.Map<IEnumerable<AiReport>,List<AiReportDTO>>(ReportsList);
                    return new ResultResponse<List<AiReportDTO>>
                    {
                        ISucsses = true,
                        Obj = ReportDTO
                    };
            }
            
            return new ResultResponse<List<AiReportDTO>>
            {
                ISucsses = false,
                Message="Not Found Any Reports ...!"
            };
        }
    }
}
