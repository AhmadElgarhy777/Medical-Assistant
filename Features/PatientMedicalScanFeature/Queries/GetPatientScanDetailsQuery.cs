using MediatR;
using Models;

namespace Features.PatientMedicalScanFeature.Queries
{
    public record GetPatientScanDetailsQuery(string ScanId, string UserId) : IRequest<ResultResponse<PatientMedicalScan>>;
}
