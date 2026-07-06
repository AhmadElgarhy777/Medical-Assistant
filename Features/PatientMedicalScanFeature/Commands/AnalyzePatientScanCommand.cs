using MediatR;
using Models;

namespace Features.PatientMedicalScanFeature.Commands
{
    public record AnalyzePatientScanCommand(
        string ScanId,
        string DoctorId,
        string? DoctorNote
    ) : IRequest<ResultResponse<AiReport>>;
}
