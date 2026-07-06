using System.Collections.Generic;
using MediatR;
using Models;

namespace Features.PatientMedicalScanFeature.Queries
{
    public record GetPendingScansForDoctorQuery(string DoctorId) : IRequest<ResultResponse<List<PatientMedicalScan>>>;
}
