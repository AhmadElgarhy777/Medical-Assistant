using MediatR;
using Models.DTOs.AiServicesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Commands.AnalyzeRequestedScanOrcastraor
{
    public record AnalyzeRequestedScanCommand(
    string ScanRequestId
) : IRequest<ResultResponse<AiAnalysisResultDTO>>;
}
