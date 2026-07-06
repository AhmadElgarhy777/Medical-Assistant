using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Features.RegisterationFeature.Commands
{
    public record ApproveLabCommand(string LabId, bool Approve) : IRequest<ResultResponse<string>>;
}