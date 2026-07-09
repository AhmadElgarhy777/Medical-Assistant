using Features.PatientScanRequet.Dtos;
using MediatR;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Queries.GetPatientScanRequests
{
    public record GetPatientScanRequestsQuery(
    ScanRequestStatus? Status = null)
    : IRequest<ResultResponse<List<PatientScanRequestDto>>>;
}
