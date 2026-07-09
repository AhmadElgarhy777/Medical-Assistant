using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Commands.RejectUploadedScan
{
    public record RejectUploadedScanCommand(
      string ScanRequestId,
      string RejectReason
  ) : IRequest<ResultResponse<string>>;
}
