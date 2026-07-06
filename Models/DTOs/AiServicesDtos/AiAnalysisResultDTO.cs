using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.AiServicesDtos
{
    public class AiAnalysisResultDTO
    {
        public double Confidence { get; set; }

        public string Prediction { get; set; } = null!;
           
    }

}
