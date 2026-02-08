using MediatR;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AdminFeature.Commands
{
    public record ChangeStatusCommand(string userId, ConfrmationStatus status,CancellationToken CancellationToken):IRequest<ResultResponse<String>>;
}
