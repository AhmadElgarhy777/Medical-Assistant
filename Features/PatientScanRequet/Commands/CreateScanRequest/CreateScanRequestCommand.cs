using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Commands.CreateScanRequest
{
    public record CreateScanRequestCommand(
     string PatientId,
     AiModelTypeEnum AIModelType,
     string? DoctorNote,
     DateTime? ExpirationDate
 ) : IRequest<ResultResponse<string>>;
}
