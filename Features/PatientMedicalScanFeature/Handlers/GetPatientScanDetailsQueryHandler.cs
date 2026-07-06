using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Repositry.IRepositry;
using Features.PatientMedicalScanFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Features.PatientMedicalScanFeature.Handlers
{
    public class GetPatientScanDetailsQueryHandler : IRequestHandler<GetPatientScanDetailsQuery, ResultResponse<PatientMedicalScan>>
    {
        private readonly IPatientMedicalScanRepositry _scanRepository;

        public GetPatientScanDetailsQueryHandler(IPatientMedicalScanRepositry scanRepository)
        {
            _scanRepository = scanRepository;
        }

        public async Task<ResultResponse<PatientMedicalScan>> Handle(GetPatientScanDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var scan = await _scanRepository.GetTable()
                    .Include(s => s.Patient)
                    .Include(s => s.Doctor)
                    .Include(s => s.AiReport)
                        .ThenInclude(r => r!.Images)
                    .FirstOrDefaultAsync(s => s.ID == request.ScanId, cancellationToken);

                if (scan == null)
                {
                    return new ResultResponse<PatientMedicalScan>
                    {
                        ISucsses = false,
                        Message = "Scan not found."
                    };
                }

                // Ensure the caller is either the patient or the assigned doctor
                if (scan.PatientId != request.UserId && scan.DoctorId != request.UserId)
                {
                    return new ResultResponse<PatientMedicalScan>
                    {
                        ISucsses = false,
                        Message = "Unauthorized. You do not have access to this scan."
                    };
                }

                return new ResultResponse<PatientMedicalScan>
                {
                    ISucsses = true,
                    Message = "Scan details retrieved successfully.",
                    Obj = scan
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<PatientMedicalScan>
                {
                    ISucsses = false,
                    Message = "An error occurred while retrieving scan details.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
