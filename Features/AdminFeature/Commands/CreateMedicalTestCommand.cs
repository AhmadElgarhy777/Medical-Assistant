using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Features.AdminFeature.Commands
{
    public record CreateMedicalTestCommand(
        string Name,
        string Description,
        string Category,
        decimal BasePrice,
        int TurnaroundHours,
        string PreparationInstructions,
        bool RequiresFasting
    ) : IRequest<ResultResponse<string>>;
}