using DataAccess.Repositry.IRepositry;
using Features.PatientScanRequet.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Queries.GetScanRequestDetails
{
    public class GetScanRequestDetailsQueryHandler
     : IRequestHandler<GetScanRequestDetailsQuery, ResultResponse<ScanRequestDetailsDto>>
    {
        private readonly IScanRequestRepository repository;

        public GetScanRequestDetailsQueryHandler(
            IScanRequestRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ResultResponse<ScanRequestDetailsDto>> Handle(
            GetScanRequestDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var scanRequest = await repository
                .GetTable()
                .Include(x => x.Patient)
                .Include(x => x.Images)
                .Include(x=>x.AiReport)
                .FirstOrDefaultAsync(x => x.ID == request.ScanRequestId, cancellationToken);

            if (scanRequest is null)
            {
                return new ResultResponse<ScanRequestDetailsDto>
                {
                    ISucsses = false,
                    Message = "Scan Request Not Found."
                };
            }

            var dto = new ScanRequestDetailsDto
            {
                ScanRequestId = scanRequest.ID,
                PatientId = scanRequest.PatientId,
                PatientName = scanRequest.Patient.UserName,
                AIModelType = scanRequest.AIModelType,
                Status = scanRequest.Status,
                DoctorNote = scanRequest.DoctorNote,
                ExpirationDate = scanRequest.ExpirationDate,
                AiReport=scanRequest.AiReport ==null ? null :new AiReportForScaanDto
                {
                    ReportId = scanRequest.AiReport.ID,
                    Diagnosis = scanRequest.AiReport.Diagnosis,
                    Confidence = scanRequest.AiReport.Confidence,
                    DoctorNote = scanRequest.AiReport.DoctorNote,
                    ModelType = scanRequest.AiReport.ModelType
                },
                Images = scanRequest.Images
                                    .Select(x => new ScanRequestImageDto
                                    {
                                        ImageId = x.ID,
                                        ImagePath = x.ImagePath
                                    }).ToList()
            };
            

            return new ResultResponse<ScanRequestDetailsDto>
            {
                ISucsses = true,
                Obj = dto
            };
        }
    }
}
