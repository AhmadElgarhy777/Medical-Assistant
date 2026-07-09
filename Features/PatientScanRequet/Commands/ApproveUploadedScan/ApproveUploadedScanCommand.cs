using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Commands.ApproveUploadedScan
{
    public record ApproveUploadedScanCommand(
     string ScanRequestId
 ) : IRequest<ResultResponse<string>>;
}
