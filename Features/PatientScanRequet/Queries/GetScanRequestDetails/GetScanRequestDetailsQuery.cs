using Features.PatientScanRequet.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Queries.GetScanRequestDetails
{
    public record GetScanRequestDetailsQuery(string ScanRequestId)
     : IRequest<ResultResponse<ScanRequestDetailsDto>>;
}
