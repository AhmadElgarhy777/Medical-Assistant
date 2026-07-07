//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading;
//using System.Threading.Tasks;
//using DataAccess.Repositry.IRepositry;
//using DataAccess.UnitOfWork;
//using Features.AiFeature.AnalyzeBrainTumorFeature;
//using Features.AiFeature.CBCBloodTest;
//using Features.AiFeature.ChestRayClassifcation;
//using Features.AiFeature.SharedMethod;
//using Features.AiFeature.SkinCancerClassification;
//using Features.PatientMedicalScanFeature.Commands;
//using MediatR;
//using Microsoft.AspNetCore.Hosting;
//using Models;
//using Models.Enums;

//namespace Features.PatientMedicalScanFeature.Handlers
//{
//    public class AnalyzePatientScanCommandHandler : IRequestHandler<AnalyzePatientScanCommand, ResultResponse<AiReport>>
//    {
//        private readonly IPatientMedicalScanRepositry _scanRepository;
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IWebHostEnvironment _environment;

//        public AnalyzePatientScanCommandHandler(
//            IPatientMedicalScanRepositry scanRepository,
//            IUnitOfWork unitOfWork,
//            IWebHostEnvironment environment)
//        {
//            _scanRepository = scanRepository;
//            _unitOfWork = unitOfWork;
//            _environment = environment;
//        }

//        public async Task<ResultResponse<AiReport>> Handle(AnalyzePatientScanCommand request, CancellationToken cancellationToken)
//        {
//            // 1. Fetch scan request
//            var scan = await _scanRepository.GetTable()
//                .FindAsync(new object[] { request.ScanId }, cancellationToken);

//            if (scan == null)
//            {
//                return new ResultResponse<AiReport>
//                {
//                    ISucsses = false,
//                    Message = "Patient scan not found."
//                };
//            }

//            if (scan.DoctorId != request.DoctorId)
//            {
//                return new ResultResponse<AiReport>
//                {
//                    ISucsses = false,
//                    Message = "Unauthorized. You are not the assigned doctor for this scan."
//                };
//            }

//            if (scan.Status != MedicalScanStatusEnum.PendingDoctorReview)
//            {
//                return new ResultResponse<AiReport>
//                {
//                    ISucsses = false,
//                    Message = $"Scan is already in '{scan.Status}' state and cannot be analyzed."
//                };
//            }

//            var absoluteImagePath = Path.Combine(_environment.WebRootPath, scan.ImagePath);
//            if (!File.Exists(absoluteImagePath))
//            {
//                return new ResultResponse<AiReport>
//                {
//                    ISucsses = false,
//                    Message = "The saved scan image file could not be found on the server."
//                };
//            }

//            // 2. Call orchestrator to perform AI prediction and save the report
//            var orchestratorResult = await _orchestrator.ProcessAnalysisAsync(
//                new List<string> { absoluteImagePath },
//                scan.PatientId,
//                scan.DoctorId,
//                request.DoctorNote ?? scan.DoctorNote,
//                scan.ModelType,
//                cancellationToken);

//            if (!orchestratorResult.ISucsses)
//            {
//                return orchestratorResult;
//            }

//            // 3. Update scan request status and associate report
//            scan.Status = MedicalScanStatusEnum.Analyzed;
//            scan.AiReportId = orchestratorResult.Obj.ID;
//            if (!string.IsNullOrEmpty(request.DoctorNote))
//            {
//                scan.DoctorNote = request.DoctorNote;
//            }

//            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
//            try
//            {
//                _scanRepository.Edit(scan);
//                await _unitOfWork.CompleteAsync(cancellationToken);
//                await transaction.CommitAsync(cancellationToken);

//                return orchestratorResult;
//            }
//            catch (Exception ex)
//            {
//                await transaction.RollbackAsync(cancellationToken);
//                return new ResultResponse<AiReport>
//                {
//                    ISucsses = false,
//                    Message = $"An error occurred while linking the AI report: {ex.Message}",
//                    Errors = new List<string> { ex.Message }
//                };
//            }
//        }
//    }
//}
