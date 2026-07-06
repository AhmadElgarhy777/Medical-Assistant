using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public record LabListItemDTO(
        string Id,
        string Name,
        string Address,
        string AreaName,
        double Rating,
        int ReviewsCount,
        string? ImageUrl,
        string WorkingHours,
        bool SupportsHomeCollection);
}