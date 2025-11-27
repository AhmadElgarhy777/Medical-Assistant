using AutoMapper;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using Features.PatientFeature.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Handler
{
    public class GetAiReportHandler : IRequestHandler<GetAiReportsQuery, List<AiReportDTO>>
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
        public async Task<List<AiReportDTO>> Handle(GetAiReportsQuery request, CancellationToken cancellationToken)
        {
            var patientId = request.Id;
            var page = request.page;
            if (page <= 0) page = 1;

            var Reports =  reportRepositry.GetAll(expression: e => e.PatientId == patientId, includeProp: [e => e.Doctor,e=>e.Doctor.Specialization]);
            List<AiReportDTO> ReportDTO = new List<AiReportDTO>();
            if (Reports != null)
            {
                Reports = Reports.Skip((page - 1) * 5).Take(5);
                var ReportsList = await Reports.ToListAsync(cancellationToken);
                foreach (var app in ReportsList)
                {
                    ReportDTO.Add(mapper.Map<AiReportDTO>(app));
                }

            }
            return ReportDTO;
        }
    }
}
