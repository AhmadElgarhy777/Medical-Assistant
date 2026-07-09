using Models.DTOs.AiServicesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AiFeature.SharedMethod.AiReportService
{
    public class AiReportResultDto
    {
        public string ReportId { get; set; } = default!;
        public AiAnalysisResultDTO AnalysisResult { get; set; } = default!;
    }
}
