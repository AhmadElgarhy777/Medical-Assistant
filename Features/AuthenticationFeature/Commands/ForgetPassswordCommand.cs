using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Commands
{
    public record ForgetPassswordCommand(string Email,CancellationToken CancellationToken):IRequest<ResultResponse<String>>;
}
