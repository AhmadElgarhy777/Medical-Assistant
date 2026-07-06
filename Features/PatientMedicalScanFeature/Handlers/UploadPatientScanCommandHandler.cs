using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.PatientMedicalScanFeature.Commands;
using MediatR;
using Models;
using Models.Enums;
using Services.ImageServices;

namespace Features.PatientMedicalScanFeature.Handlers
{
    public class UploadPatientScanCommandHandler : IRequestHandler<UploadPatientScanCommand, ResultResponse<PatientMedicalScan>>
    {
        private readonly IPatientMedicalScanRepositry _scanRepository;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;

        public UploadPatientScanCommandHandler(
            IPatientMedicalScanRepositry scanRepository,
            IImageService imageService,
            IUnitOfWork unitOfWork)
        {
            _scanRepository = scanRepository;
            _imageService = imageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultResponse<PatientMedicalScan>> Handle(UploadPatientScanCommand request, CancellationToken cancellationToken)
        {
            if (request.ScanFile == null || request.ScanFile.Length == 0)
            {
                return new ResultResponse<PatientMedicalScan>
                {
                    ISucsses = false,
                    Message = "No file uploaded or file is empty.",
                    Errors = new List<string> { "File is required." }
                };
            }

            var extension = Path.GetExtension(request.ScanFile.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".dcm", ".dicom" };

            if (!allowedExtensions.Contains(extension))
            {
                return new ResultResponse<PatientMedicalScan>
                {
                    ISucsses = false,
                    Message = "Invalid file extension.",
                    Errors = new List<string> { $"Allowed extensions are: {string.Join(", ", allowedExtensions)}" }
                };
            }

            // 1. Upload/Save scan image to Storage
            string fileName = await _imageService.UploadImgAsync(request.ScanFile, "Storage", cancellationToken);
            string savedPath = Path.Combine("Storage", fileName);

            // 2. Create PatientMedicalScan record
            var scan = new PatientMedicalScan
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                ModelType = request.ModelType,
                ImagePath = savedPath,
                Status = MedicalScanStatusEnum.PendingDoctorReview,
                CreatedAt = DateTime.Now
            };

            // 4. Save to database
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                _scanRepository.Add(scan);
                await _unitOfWork.CompleteAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new ResultResponse<PatientMedicalScan>
                {
                    ISucsses = true,
                    Message = "Scan uploaded successfully.",
                    Obj = scan
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ResultResponse<PatientMedicalScan>
                {
                    ISucsses = false,
                    Message = "An error occurred while saving the scan.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
