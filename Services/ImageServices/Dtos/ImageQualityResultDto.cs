using System.Collections.Generic;

namespace Services.ImageServices.Dtos
{
    public class ImageQualityResultDto
    {
        public bool IsValid { get; set; }
        public double BlurScore { get; set; }
        public double Brightness { get; set; }
        public double Contrast { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
