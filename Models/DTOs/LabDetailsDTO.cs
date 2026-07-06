using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public record LabDetailsDTO(
        string Id,
        string Name,
        string Address,
        string Phone,
        double Rating,
        int ReviewsCount,
        string WorkingHours,
        List<MedicalTestCardDTO> Tests);
}