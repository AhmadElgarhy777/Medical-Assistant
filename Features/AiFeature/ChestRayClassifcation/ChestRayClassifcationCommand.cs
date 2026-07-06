using MediatR;
using Microsoft.AspNetCore.Http;
using Models.DTOs.AiServicesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AiFeature.ChestRayClassifcation
{
      public record ChestRayClassifcationCommand(List<IFormFile> Images, string PatientId, string DoctorId, string? DoctorNote)
: IRequest<ResultResponse<AiAnalysisResultDTO>>;
}
