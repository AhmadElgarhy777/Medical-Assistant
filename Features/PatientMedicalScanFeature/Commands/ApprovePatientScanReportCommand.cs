using MediatR;
using Models;

namespace Features.PatientMedicalScanFeature.Commands
{
    public record ApprovePatientScanReportCommand(
        string ScanId,
        string DoctorId,
        string? DoctorNote
    ) : IRequest<ResultResponse<PatientMedicalScan>>;
}
