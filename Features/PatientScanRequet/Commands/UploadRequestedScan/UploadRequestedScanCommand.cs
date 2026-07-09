using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Commands.UploadRequestedScan
{
    public record UploadRequestedScanCommand(
    string ScanRequestId,
    List<IFormFile> Images
) : IRequest<ResultResponse<string>>;
}
