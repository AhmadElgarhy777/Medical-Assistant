using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.AiServicesDtos
{
    public class ImagePredictionDTO
    {
        public string FileName { get; set; } = null!;

        public string Diagnosis { get; set; } = null!;

        public double Confidence { get; set; }
    }
    
}
