using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Commands
{
    public record LogOutCommand(string RefreshToken, string IpAddress, string DeviceInfo, CancellationToken CancellationToken) :IRequest<bool>;
    
}
