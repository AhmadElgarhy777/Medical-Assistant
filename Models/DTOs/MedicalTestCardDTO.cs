using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public record MedicalTestCardDTO(
        string TestId,
        string Name,
        string Category,
        decimal Price,
        int TurnaroundHours,
        bool RequiresFasting,
        string Description,
        string PreparationInstructions);
}