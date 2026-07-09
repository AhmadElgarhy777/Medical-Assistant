using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Commands.PublishAiReport
{
    public record PublishAiReportCommand(
    string ScanRequestId,
    string? DoctorNote
) : IRequest<ResultResponse<string>>;
}
