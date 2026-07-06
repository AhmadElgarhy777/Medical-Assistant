using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.NurseFeature.Command
{
    public record RemoveNurseServiceCommand(string NurseId, string ServiceId)
     : IRequest<ResultResponse<string>>;
}
