using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.PatientMedicalScanFeature.Commands;
using MediatR;
using Models;
using Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Features.PatientMedicalScanFeature.Handlers
{
    public class ApprovePatientScanReportCommandHandler : IRequestHandler<ApprovePatientScanReportCommand, ResultResponse<PatientMedicalScan>>
    {
        private readonly IPatientMedicalScanRepositry _scanRepository;
        private readonly IAiReportRepositry _reportRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApprovePatientScanReportCommandHandler(
            IPatientMedicalScanRepositry scanRepository,
            IAiReportRepositry reportRepository,
            IUnitOfWork unitOfWork)
        {
            _scanRepository = scanRepository;
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultResponse<PatientMedicalScan>> Handle(ApprovePatientScanReportCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Retrieve the scan with its report
                var scan = await _scanRepository.GetTable()
                    .Include(s => s.AiReport)
                    .FirstOrDefaultAsync(s => s.ID == request.ScanId, cancellationToken);

                if (scan == null)
                {
                    return new ResultResponse<PatientMedicalScan>
                    {
                        ISucsses = false,
                        Message = "Patient scan not found."
                    };
                }

                if (scan.DoctorId != request.DoctorId)
                {
                    return new ResultResponse<PatientMedicalScan>
                    {
                        ISucsses = false,
                        Message = "Unauthorized. You are not the assigned doctor for this scan."
                    };
                }

                if (scan.Status != MedicalScanStatusEnum.Analyzed)
                {
                    return new ResultResponse<PatientMedicalScan>
                    {
                        ISucsses = false,
                        Message = "Only scans that have been analyzed by AI can be approved."
                    };
                }

                // Update scan properties
                scan.Status = MedicalScanStatusEnum.Approved;
                scan.ApprovedAt = DateTime.Now;

                if (!string.IsNullOrEmpty(request.DoctorNote))
                {
                    scan.DoctorNote = request.DoctorNote;
                    if (scan.AiReport != null)
                    {
                        scan.AiReport.DoctorNote = request.DoctorNote;
                        _reportRepository.Edit(scan.AiReport);
                    }
                }

                _scanRepository.Edit(scan);

                await _unitOfWork.CompleteAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new ResultResponse<PatientMedicalScan>
                {
                    ISucsses = true,
                    Message = "Scan report has been reviewed and approved successfully.",
                    Obj = scan
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ResultResponse<PatientMedicalScan>
                {
                    ISucsses = false,
                    Message = $"An error occurred during approval: {ex.Message}",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
