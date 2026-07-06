using MediatR;
using Microsoft.AspNetCore.Http;
using Models;
using Models.Enums;

namespace Features.PatientMedicalScanFeature.Commands
{
    public record UploadPatientScanCommand(
        string PatientId,
        string DoctorId,
        AiModelTypeEnum ModelType,
        IFormFile ScanFile
    ) : IRequest<ResultResponse<PatientMedicalScan>>;
}
