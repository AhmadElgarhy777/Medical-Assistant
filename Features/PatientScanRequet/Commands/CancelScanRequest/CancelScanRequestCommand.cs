using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Commands.CancelScanRequest
{
    public record CancelScanRequestCommand(
     string ScanRequestId,
     string? CancelReason
 ) : IRequest<ResultResponse<string>>;
}
