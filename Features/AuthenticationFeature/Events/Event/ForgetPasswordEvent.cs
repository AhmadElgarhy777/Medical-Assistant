using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Events.Event
{
    public record ForgetPasswordEvent(string UserId,string Email,string Token,CancellationToken CancellationToken):INotification;
}
