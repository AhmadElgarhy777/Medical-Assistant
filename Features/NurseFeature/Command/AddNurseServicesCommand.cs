using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.NurseFeature.Command
{
    public record AddNurseServicesCommand(
     string NurseId,
     List<string> ServiceIds
 ) : IRequest<ResultResponse<string>>;  
}
