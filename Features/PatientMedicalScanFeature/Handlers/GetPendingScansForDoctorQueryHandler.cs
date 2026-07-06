using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Repositry.IRepositry;
using Features.PatientMedicalScanFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;

namespace Features.PatientMedicalScanFeature.Handlers
{
    public class GetPendingScansForDoctorQueryHandler : IRequestHandler<GetPendingScansForDoctorQuery, ResultResponse<List<PatientMedicalScan>>>
    {
        private readonly IPatientMedicalScanRepositry _scanRepository;

        public GetPendingScansForDoctorQueryHandler(IPatientMedicalScanRepositry scanRepository)
        {
            _scanRepository = scanRepository;
        }

        public async Task<ResultResponse<List<PatientMedicalScan>>> Handle(GetPendingScansForDoctorQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var scans = await _scanRepository.GetTable()
                    .Include(s => s.Patient)
                    .Where(s => s.DoctorId == request.DoctorId && 
                               (s.Status == MedicalScanStatusEnum.PendingDoctorReview || 
                                s.Status == MedicalScanStatusEnum.Analyzed))
                    .OrderByDescending(s => s.CreatedAt)
                    .ToListAsync(cancellationToken);

                return new ResultResponse<List<PatientMedicalScan>>
                {
                    ISucsses = true,
                    Message = "Pending scans retrieved successfully.",
                    Obj = scans
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<List<PatientMedicalScan>>
                {
                    ISucsses = false,
                    Message = "An error occurred while retrieving pending scans.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
