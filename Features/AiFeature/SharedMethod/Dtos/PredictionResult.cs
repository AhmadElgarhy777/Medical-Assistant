using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AiFeature.SharedMethod.Dtos
{
    public class PredictionResult
    {
        public string Prediction { get; set; } = null!;
        public double Confidence { get; set; }
    }
}
